using PagedList;
using System.Collections.Generic;

namespace BWYou.Web.MVC.ViewModels
{
    /// <summary>
    /// 커서 형태의 정보 저장용 클래스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CursorResultViewModel<T>
    {
        /// <summary>
        /// 페이지 결과 객체
        /// </summary>
        public IEnumerable<T> Result { get; set; }
        /// <summary>
        /// 페이지 메타 정보
        /// </summary>
        public CursorMetaData<T> MetaData { get; set; }
        /// <summary>
        /// 기본 생성자
        /// </summary>
        public CursorResultViewModel()
        {

        }
        /// <summary>
        /// 결과 객체 저장용 생성자. 메타 정보는 직접 주입 필요.
        /// </summary>
        /// <param name="result"></param>
        public CursorResultViewModel(IEnumerable<T> result)
        {
            this.Result = result;
        }
        /// <summary>
        /// 결과 객체와 메타 정보를 한번에 저장하기 위한 생성자
        /// </summary>
        /// <param name="result"></param>
        /// <param name="MetaData"></param>
        public CursorResultViewModel(IEnumerable<T> result, CursorMetaData<T> MetaData)
        {
            this.Result = result;
            this.MetaData = MetaData;
        }
    }
}
