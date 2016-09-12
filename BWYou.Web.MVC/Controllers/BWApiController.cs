using BWYou.Web.MVC.Models;
using BWYou.Web.MVC.Services;
using BWYou.Web.MVC.ViewModels;
using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.ModelBinding;

namespace BWYou.Web.MVC.Controllers
{
    /// <summary>
    /// 공통으로 사용 할 수 있도록 genetic을 활용 하여 공통 모델에 대한 Restful 기능을 구현
    /// 단 여기서 직접적으로 호출 되지 않도록 함수 앞에 Base를 붙이고, protected 처리 해서 기능을 상속 받은 자식이 호출 해서 사용하도록 처리
    /// </summary>
    /// <typeparam name="TEntity">기본 모델 엔티티</typeparam>
    public class BWApiController<TEntity> : ApiController
        where TEntity : BWModel
    {
        protected IEntityService<TEntity> _service;

        protected int pageSize = 15;

        public BWApiController(DbContext dbContext)
            : this(new BWEntityService<TEntity>(dbContext))
        {

        }

        public BWApiController(BWEntityService<TEntity> service)
        {
            this._service = service;
        }

        protected virtual HttpResponseMessage BaseGetList()
        {
            var models = this._service.GetList();

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual async Task<HttpResponseMessage> BaseGetListAsync()
        {
            var models = await this._service.GetListAsync();

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual HttpResponseMessage BaseGetList(string sort)
        {
            var models = this._service.GetList(sort);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual async Task<HttpResponseMessage> BaseGetListAsync(string sort)
        {
            var models = await this._service.GetListAsync(sort);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual HttpResponseMessage BaseGetList(int page, string sort)
        {
            var models = new PageResultViewModel<TEntity>(this._service.GetList(sort, page, pageSize));

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual async Task<HttpResponseMessage> BaseGetListAsync(int page, string sort)
        {
            var models = new PageResultViewModel<TEntity>(await this._service.GetListAsync(sort, page, pageSize));

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual HttpResponseMessage BaseGetFilteredList(TEntity searchModel)
        {
            var models = this._service.GetFilteredList(searchModel);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel)
        {
            var models = await this._service.GetFilteredListAsync(searchModel);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual HttpResponseMessage BaseGetFilteredList(TEntity searchModel, string sort)
        {
            var models = this._service.GetFilteredList(searchModel, sort);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel, string sort)
        {
            var models = await this._service.GetFilteredListAsync(searchModel, sort);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual HttpResponseMessage BaseGetFilteredList(TEntity searchModel, int page, string sort)
        {
            var models = new PageResultViewModel<TEntity>(this._service.GetFilteredList(searchModel, sort, page, pageSize));

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel, int page, string sort)
        {
            var models = new PageResultViewModel<TEntity>(await this._service.GetFilteredListAsync(searchModel, sort, page, pageSize));

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected virtual HttpResponseMessage BaseGet(int id)
        {
            var model = this._service.Get(id);

            if (model != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        protected virtual async Task<HttpResponseMessage> BaseGetAsync(int id)
        {
            var model = await this._service.GetAsync(id);

            if (model != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        protected virtual HttpResponseMessage BasePost(TEntity model)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                var retModel = this._service.ValidAndCreate(model, ModelState);

                if (retModel != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, retModel);
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }

        protected virtual async Task<HttpResponseMessage> BasePostAsync(TEntity model)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                var retModel = await this._service.ValidAndCreateAsync(model, ModelState);

                if (retModel != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, retModel);
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }

        protected virtual HttpResponseMessage BasePut(int id, TEntity model)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                model.Id = id;
                var retModel = this._service.ValidAndUpdate(model, ModelState);

                if (retModel != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, retModel);
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }

        protected virtual async Task<HttpResponseMessage> BasePutAsync(int id, TEntity model)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                model.Id = id;
                var retModel = await this._service.ValidAndUpdateAsync(model, ModelState);

                if (retModel != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, retModel);
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }

        protected virtual HttpResponseMessage BaseDelete(int id)
        {
            var retModel = this._service.ValidAndDelete(id, ModelState);

            if (retModel != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, retModel);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }

        protected virtual async Task<HttpResponseMessage> BaseDeleteAsync(int id)
        {
            var retModel = await this._service.ValidAndDeleteAsync(id, ModelState);

            if (retModel != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, retModel);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }

        protected virtual HttpResponseMessage BaseClone(int id)
        {
            var retModel = this._service.ValidAndClone(id, ModelState);

            if (retModel != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, retModel);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }

        protected virtual async Task<HttpResponseMessage> BaseCloneAsync(int id)
        {
            var retModel = await this._service.ValidAndCloneAsync(id, ModelState);

            if (retModel != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, retModel);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._service.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Web Api 용 리밸리데이션
        /// http://stackoverflow.com/questions/12906359/revalidate-model-when-using-webapi-tryvalidatemode-equivalent
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected internal bool TryValidateModel(object model)
        {
            return TryValidateModel(model, null /* prefix */);
        }

        protected internal bool TryValidateModel(object model, string prefix)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            var t = new ModelBindingExecutionContext(new HttpContextWrapper(HttpContext.Current), new System.Web.ModelBinding.ModelStateDictionary());

            foreach (ModelValidationResult validationResult in ModelValidator.GetModelValidator(metadata, t).Validate(null))
            {
                ModelState.AddModelError(validationResult.MemberName, validationResult.Message);
            }

            return ModelState.IsValid;
        }
    }
}
