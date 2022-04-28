using System.Collections;

namespace Furion.Extras.Admin.NET.Entity.Common.Tree
{
    public class ProjectTreeNode : ITreeNode
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<ProjectTreeNode> Children { get; set; } = new List<ProjectTreeNode>();

        /// <summary>
        /// 上一级Id
        /// </summary>
        public long Pid { get; set; }

        public long GetId()
        {
            return Id;
        }

        public long GetPid()
        {
            return ParentId;
        }

        public void SetChildren(IList children)
        {
            Children = (List<ProjectTreeNode>)children;
        }
    }
}
