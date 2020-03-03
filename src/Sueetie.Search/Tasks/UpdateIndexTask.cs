using System.Xml.Linq;
using Sueetie.Core;

namespace Sueetie.Search.Tasks
{
    public class UpdateIndexTask : ISueetieTask
    {
        #region ITask Members

        public void Execute(XElement _xTaskElement)
        {
            //SueetieIndexTaskItem _taskItem = SueetieSearch.GetSueetieIndexTaskItem((int)SueetieTaskType.BuildSearchIndex);
            //SueetieSearch _sueetieSearch = new SueetieSearch();
            //_taskItem.TaskTypeID = (int)SueetieTaskType.BuildSearchIndex;
            //_sueetieSearch.UpdateIndex(_taskItem);
        }

        #endregion
    }
}
