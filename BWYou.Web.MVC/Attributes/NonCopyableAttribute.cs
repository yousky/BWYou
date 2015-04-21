using System;

namespace BWYou.Web.MVC.Attributes
{
    /// <summary>
    /// Copy 되어 질 수 없는 칼럼임을 나타냄
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NonCopyableAttribute : Attribute
    {
        public readonly bool OnlyWhenCreate;

        public NonCopyableAttribute()
        {
            OnlyWhenCreate = false;
        }

        public NonCopyableAttribute(bool OnlyWhenCreate)
        {
            this.OnlyWhenCreate = OnlyWhenCreate;
        }
    }
}
