using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService: GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
        {
            _platformRepo = repo;
            _mapper = mapper;

        }

       public override async Task<PlatformResponse> GetAllPlatforms(
           GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = await _platformRepo.GetAllFlatformsAsync();
            foreach (var platform in platforms)
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform)); 
            return response;
        }
    }
}
