using BWYou.Web.MVC.Models;
using BWYou.Web.MVC.Services;
using BWYou.Web.MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.Controllers
{
    public class IdApiVMController<TEntity, TId, TVM> : IdApiController<TEntity, TId>
        where TEntity : BWModel<TId>
        where TVM : IModelLoader<TEntity>, new()
    {
        public IdApiVMController(DbContext dbContext)
            : base(dbContext)
        {

        }
        public IdApiVMController(BWEntityService<TEntity, TId> service)
            : base(service)
        {

        }
        //TODO ConverVM 함수 다른 곳으로 뺄 수 있는지 고민해 보자
        public static async Task<List<TVM>> ConvertVMAsync(IEnumerable<TEntity> baseModels, string sort = "Id", int depth = 0)
        {
            return await Task<List<TVM>>.Run(() =>
            {
                List<TVM> models = new List<TVM>();

                foreach (var baseModel in baseModels)
                {
                    TVM model = new TVM();
                    model.LoadModel(baseModel, 0, depth, sort);
                    models.Add(model);
                }
                return models;
            });
        }
        public static async Task<TVM> ConvertVMAsync(TEntity baseModel, string sort = "Id", int depth = 0)
        {
            TVM model = new TVM();
            await model.LoadModelAsync(baseModel, 0, depth, sort);
            return model;
        }


        protected override async Task<HttpResponseMessage> BaseGetListAsync()
        {
            IEnumerable<TEntity> baseModels = await this._service.GetListAsync();

            List<TVM> models = await ConvertVMAsync(baseModels, "Id", 0);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        protected override async Task<HttpResponseMessage> BaseGetListAsync(string sort)
        {
            IEnumerable<TEntity> baseModels = await this._service.GetListAsync(sort);

            List<TVM> models = await ConvertVMAsync(baseModels, sort, 0);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        protected override Task<HttpResponseMessage> BaseGetListAsync(int page, string sort)
        {
            return base.BaseGetListAsync(page, sort);
        }
        protected override async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel)
        {
            IEnumerable<TEntity> baseModels = await this._service.GetFilteredListAsync(searchModel);

            List<TVM> models = await ConvertVMAsync(baseModels, "Id", 0);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }
        protected override Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel, string sort)
        {
            return BaseGetFilteredListAsync(searchModel, sort, 0);
        }

        protected override Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel, int page, string sort)
        {
            return BaseGetFilteredListAsync(searchModel, page, sort, 0);
        }

        protected async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel, string sort, int depth)
        {
            IEnumerable<TEntity> baseModels = await this._service.GetFilteredListAsync(searchModel, sort);

            List<TVM> models = await ConvertVMAsync(baseModels, sort, depth);

            return Request.CreateResponse(HttpStatusCode.OK, models);
        }

        protected async Task<HttpResponseMessage> BaseGetFilteredListAsync(TEntity searchModel, int page, string sort, int depth)
        {
            var baseModels = await this._service.GetFilteredListAsync(searchModel, sort, page, pageSize);

            List<TVM> models = await ConvertVMAsync(baseModels, sort, depth);

            MetaData metaData = new MetaData(baseModels);

            var result = new PageResultViewModel<TVM>(models, metaData);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        protected override Task<HttpResponseMessage> BaseGetAsync(TId id)
        {
            return BaseGetAsync(id, "Id", 0);
        }

        protected Task<HttpResponseMessage> BaseGetAsync(TId id, string sort)
        {
            return BaseGetAsync(id, sort, 0);
        }

        protected async Task<HttpResponseMessage> BaseGetAsync(TId id, string sort, int depth)
        {
            TEntity baseModel = await this._service.GetAsync(id);

            if (baseModel != null)
            {
                TVM model = await ConvertVMAsync(baseModel, sort, depth);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
