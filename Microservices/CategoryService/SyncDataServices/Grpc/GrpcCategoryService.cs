using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using CategoryService.Data;

namespace CategoryService.SyncDataServices.Grpc
{
    public class GrpcCategoryService : GrpcCategory.GrpcCategoryBase
    {
        private readonly ICategoryRepo _repository;
        private readonly IMapper _mapper;

        public GrpcCategoryService(ICategoryRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<CategoryResponse> GetAllCategories(GetAllRequest request, ServerCallContext context)
        {
            Console.WriteLine("--> GetAllCategories on gRPC server");
            var response = new CategoryResponse();
            var categories = _repository.GetAll();

            foreach(var item in categories)
            {
                response.Category.Add(_mapper.Map<GrpcCategoryModel>(item));
            }

            return Task.FromResult(response);
        }
    }
}