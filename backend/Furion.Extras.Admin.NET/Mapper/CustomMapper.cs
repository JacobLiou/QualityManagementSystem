using Furion.Extras.Admin.NET.Service;
using Mapster;

namespace Furion.Extras.Admin.NET
{
    public class CustomMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<SysOrg, OrgTreeNode>()
                .Map(dest => dest.ParentId, src => src.Pid)
                .Map(dest => dest.Title, src => src.Name)
                .Map(dest => dest.Value, src => src.Id)
                .Map(dest => dest.Weight, src => src.Sort);
        }
    }
}