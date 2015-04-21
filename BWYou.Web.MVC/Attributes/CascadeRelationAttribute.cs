using System;

namespace BWYou.Web.MVC.Attributes
{
    /// <summary>
    /// 순차적으로 향해 지는 프로퍼티임을 나타냄
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CascadeRelationAttribute : Attribute
    {
        /// <summary>
        /// 연결의 방향
        /// </summary>
        public CascadeDirection Direction { get; set; }
        public bool Clonable { get; set; }

        public CascadeRelationAttribute(CascadeDirection Direction)
        {
            this.Direction = Direction;
            this.Clonable = true;
        }
        /// <summary>
        /// 관계 연결의 방향
        /// </summary>
        public enum CascadeDirection
        {
            /// <summary>
            /// 부모가 자식을 바라 보는 방향
            /// </summary>
            Down,
            /// <summary>
            /// 자식이 부모를 바라 보는 방향
            /// </summary>
            Up
        }
    }
}
