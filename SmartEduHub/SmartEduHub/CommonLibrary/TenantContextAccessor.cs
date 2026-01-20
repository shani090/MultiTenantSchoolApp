using SmartEduHub.Interface;

namespace SmartEduHub.CommonLibrary
{
    public class TenantContextAccessor: ITenantContextAccessor
    {
        private string? _tenantId;

        public string? GetTenantId() => _tenantId;

        public void SetTenantId(string tenantId)
        {
            _tenantId = tenantId;
        }
    }
}
