using System;
using System.Collections.Generic;
using System.Web;
using YAF.Classes.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace YAF.Modules
{
    /// <summary>
    /// Summary description for DiceBBCodeModule
    /// </summary>
    public class DiceBBCodeModule : YAF.Controls.YafBBCodeControl
    {
        private static readonly Regex regex = new Regex(@"^((?<Label>[^:]+)\s*:)?\s*(?<Formula>(?<Roll>\d+(d\d+)?)(\s*(?<Roll>[\+\-]\d+(d\d+)?))*)$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        private static readonly Random r = new Random();

        private static readonly Dictionary<int, string> rollCache = new Dictionary<int, string>();

        private static readonly Dictionary<int, string> rollFormulaCache = new Dictionary<int, string>();

        private static bool installChecked;

        private static readonly object installLock = new object();

        public DiceBBCodeModule()
        {
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.MessageID != null)
            {
                this.EnsureInstall();

                int messageId = Convert.ToInt32(this.MessageID.Value);

                var formula = (this.Parameters["inner"] ?? string.Empty).Replace("<br/>", "").Trim();

                // Décodage HTML
                formula = HttpUtility.HtmlDecode(formula);

                writer.Write("<div class=\"diceroll\">");
                writer.Write("<ul>");
                writer.Write(this.GetOrCreateRollDice(messageId, formula));
                writer.Write("</ul>");
                writer.Write("</div>");
            }
        }

        private void EnsureInstall()
        {
            if (!installChecked)
            {
                lock (installLock)
                {
                    if (!installChecked)
                    {
                        using (var connMan = new MsSqlDbConnectionManager())
                        using (var cmd = connMan.OpenDBConnection.CreateCommand())
                        {
                            cmd.CommandText = "SELECT COUNT(*) FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_NAME] = 'jdr_diceroll'";
                            object result = cmd.ExecuteScalar();

                            if (result == DBNull.Value || result.Equals(0))
                            {
                                cmd.CommandText = @"CREATE TABLE [jdr_diceroll] ([DiceRollId] [int] NOT NULL, [Formula] VARCHAR(MAX) NOT NULL, [Result] VARCHAR(MAX) NOT NULL, CONSTRAINT [PK_jdr_diceroll] PRIMARY KEY CLUSTERED ([DiceRollId]))";
                                cmd.ExecuteNonQuery();
                            }

                            installChecked = true;
                        }
                    }
                }
            }
        }

        private string GetOrCreateRollDice(int postId, string newFormula)
        {
            string previousResult = null;
            string previousFormula = null;
            bool isNew = true;
            bool changed;

            if (rollCache.TryGetValue(postId, out previousResult))
            {
                isNew = false;
                previousFormula = rollFormulaCache[postId];

                if (previousFormula.Equals(newFormula, StringComparison.OrdinalIgnoreCase))
                {
                    return previousResult;
                }
            }

            string newResult;

            using (var connMan = new MsSqlDbConnectionManager())
            using (var cmd = connMan.OpenDBConnection.CreateCommand())
            {
                if (previousFormula == null)
                {
                    // La formule n'est pas en cache, on vérifie si elle existe en base
                    cmd.CommandText = "SELECT [Result], [Formula] FROM [jdr_diceroll] WHERE [DiceRollId] = @Id";
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Int)).Value = this.MessageID;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            previousResult = !reader.IsDBNull(0) ? reader.GetString(0) : null;
                            previousFormula = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                            isNew = false;
                        }
                        else
                        {
                            isNew = true;
                        }
                    }
                }

                if (isNew)
                {
                    // Nouvelle formule ou nouveau message
                    newResult = this.GenerateResult(null, null, newFormula, out changed);
                    if (changed)
                    {
                        cmd.CommandText = "INSERT INTO jdr_diceroll (DiceRollId, Formula, Result) VALUES (@Id, @Formula, @Result)";

                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = postId;
                        cmd.Parameters.Add(new SqlParameter("@Formula", SqlDbType.VarChar)).Value = newFormula;
                        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.VarChar)).Value = newResult;
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Modification de la formule
                    newResult = this.GenerateResult(previousFormula ?? string.Empty, previousResult ?? string.Empty, newFormula, out changed);
                    if (changed)
                    {
                        cmd.CommandText = "UPDATE jdr_diceroll SET [Formula] = @Formula, [Result] = @Result WHERE [DiceRollId] = @Id";

                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = postId;
                        cmd.Parameters.Add(new SqlParameter("@Formula", SqlDbType.VarChar)).Value = newFormula;
                        cmd.Parameters.Add(new SqlParameter("@Result", SqlDbType.VarChar)).Value = newResult;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            
            if (changed)
            {
                rollCache[postId] = newResult ?? string.Empty;
                rollFormulaCache[postId] = newFormula ?? string.Empty;
            }

            return newResult;
        }

        private string GenerateResult(string previousFormula, string previousResult, string newFormula, out bool changed)
        {
            changed = false;
            var valid = true;
            if (string.IsNullOrEmpty(previousFormula))
            {
                var res = this.RunFormula(newFormula, out valid);
                changed = valid;
                return res;
            }
            else if (newFormula.Length > previousFormula.Length && newFormula.StartsWith(previousFormula, StringComparison.OrdinalIgnoreCase))
            {
                // Ajout de jets au résultat précédent
                var newPart = newFormula.Substring(previousFormula.Length);
                var newResult = this.RunFormula(newPart, out valid);
                changed = valid;
                return previousResult + newResult;
            }
            else
            {
                // Pas de changements
                return previousResult;
            }
        }

        private string RunFormula(string formulas, out bool valid)
        {
            var result = new StringBuilder();

            var formulaArray = formulas.Trim().Split(new[] { ';', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            valid = true;

            foreach (var formula in formulaArray)
            {
                var correctFormula = formula.Trim();
                Match match = regex.Match(correctFormula);
                
                if (match.Success)
                {
                    int total = 0;
                    var count = match.Groups["Roll"].Captures.Count;

                    result.Append("<li>");
                    if (match.Groups["Label"].Success)
                    {
                        var label = match.Groups["Label"].Value.Trim();

                        while (label.StartsWith("<br />", StringComparison.Ordinal))
                            label = label.Substring(6);

                        
                        result
                            .Append("<em>")
                            .Append(label.Trim())
                            .Append("</em> : ");
                    }

                    result
                        .Append("<strong>")
                        .Append(match.Groups["Formula"].Value)
                        .Append("</strong> donne ");

                    bool first = true;
                    foreach (Capture capture in match.Groups["Roll"].Captures)
                    {
                        int dice;
                        if (first)
                        {
                            try
                            {
                                result
                                    .Append("")
                                    .Append(this.RollDice(capture.Value, out dice))
                                    .Append("");
                            }
                            catch (Exception)
                            {
                                throw new ArgumentException(string.Format("La formule {0} est invalide", capture.Value));
                            }

                            total += dice;

                            first = false;
                        }
                        else
                        {
                            var sign = capture.Value[0] == '+' ? 1 : -1;
                            result
                                .Append(" ")
                                .Append(capture.Value[0])
                                .Append(" ")
                                .Append(this.RollDice(capture.Value.Substring(1), out dice))
                                .Append("");

                            total += sign * dice;

                        }
                    }
                    
                    result.Append(" = <strong>")
                        .Append(total)
                        .Append("</strong>");
                    result.Append("</li>");
                }
                else
                {
                    result = new StringBuilder();
                    valid = false;
                    
                    result
                        .Append("<li class=\"unknown\">Formule non reconnue : <code>")
                        .Append(correctFormula)
                        .Append("</code></li>");
                        
                    break;
                }
            }

            return result.ToString();
        }

        private string RollDice(string dice, out int result)
        {
            var i = dice.IndexOf("d", StringComparison.OrdinalIgnoreCase);
            var factor = 1;
            var faces = 6;
            result = 0;

            if (i == -1)
            {
                result = int.Parse(dice);

                return dice;
            }
            else if (i != 0)
            {
                factor = int.Parse(dice.Substring(0, i));
                faces = int.Parse(dice.Substring(i + 1));
            }
            else
            {
                faces = int.Parse(dice.Substring(1));
            }

            StringBuilder builder = new StringBuilder();

            builder.Append("[<span title=\"");
            for (int diceIndex = 0; diceIndex < factor; diceIndex++)
            {
                if (diceIndex != 0)
                {
                    builder.Append(" + ");
                }

                var diceResult = r.Next(faces) + 1;
                builder.Append(diceResult);
                result += diceResult;

            }
            builder.Append("\">")
                .Append(result)
                .Append("]</span>");

            return builder.ToString();
        }
    }
}