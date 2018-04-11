using BWYou.Web.MVC.Models;
using BWYou.Web.MVC.Services;
using BWYou.Web.MVC.ViewModels;
using System;
using System.Collections.Generic;
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
    /// <typeparam name="TId"></typeparam>
    public class IdApiController<TEntity, TId> : ApiController
        where TEntity : IdModel<TId>
    {
        protected IEntityService<TEntity, TId> _service;

        protected int pageSize = 15;

        public IdApiController(DbContext dbContext)
            : this(new IdEntityService<TEntity, TId>(dbContext))
        {

        }

        public IdApiController(IdEntityService<TEntity, TId> service)
        {
            this._service = service;
        }
        /// <summary>
        /// Get all lists
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseGetListAsync()
        {
            var models = await this._service.GetListAsync();

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        /// <summary>
        /// Get all sorted lists
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseGetListAsync(string sort)
        {
            var models = await this._service.GetListAsync(sort);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        /// <summary>
        /// Get sorted paged list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseGetListAsync(int page, string sort)
        {
            var models = new PageResultViewModel<TEntity>(await this._service.GetListAsync(sort, page, pageSize));

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        /// <summary>
        /// Get all filtered lists
        /// </summary>
        /// <param name="searchModel">Search for the same item with a value</param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel)
        {
            var models = await this._service.GetFilteredListAsync(searchModel);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        /// <summary>
        /// Get all sorted filtered lists
        /// </summary>
        /// <param name="searchModel">Search for the same item with a value</param>
        /// <param name="sort"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel, string sort)
        {
            var models = await this._service.GetFilteredListAsync(searchModel, sort);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        /// <summary>
        /// Get sorted filtered paged lists
        /// </summary>
        /// <param name="searchModel">Search for the same item with a value</param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel, int page, string sort)
        {
            var models = new PageResultViewModel<TEntity>(await this._service.GetFilteredListAsync(searchModel, sort, page, pageSize));

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        /// <summary>
        /// Get a specific entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseGetAsync(TId id)
        {
            var model = await this._service.GetAsync(id);

            if (model != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        /// <summary>
        /// Create entity after validation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        protected virtual async Task<HttpResponseMessage> BasePostAsync(IEnumerable<TEntity> models)
        {
            TryValidateModel(models);
            if (ModelState.IsValid)
            {
                var retModel = await this._service.ValidAndCreateAsync(models, ModelState);

                if (retModel != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, retModel);
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }
        /// <summary>
        /// Update entity after validation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BasePutAsync(TId id, TEntity model)
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
        /// <summary>
        /// Delete entity after validation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseDeleteAsync(TId id)
        {
            var retModel = await this._service.ValidAndDeleteAsync(id, ModelState);

            if (retModel != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, retModel);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }
        protected virtual async Task<HttpResponseMessage> BaseDeleteAsync(IEnumerable<TEntity> models)
        {
            var retModel = await this._service.ValidAndDeleteAsync(models, ModelState);

            if (retModel != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, retModel);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorResultViewModel(HttpStatusCode.BadRequest, ModelState));
        }
        /// <summary>
        /// Clone entity after validation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<HttpResponseMessage> BaseCloneAsync(TId id)
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
        /// Revalidation for Web Api
        /// http://stackoverflow.com/questions/12906359/revalidate-model-when-using-webapi-tryvalidatemode-equivalent
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected internal bool TryValidateModel(object model)
        {
            return TryValidateModel(model, null /* prefix */);
        }
        /// <summary>
        /// Revalidation for Web Api
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
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
