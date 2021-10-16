using Newtonsoft.Json;

namespace StatefulEntities
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BankAccountState
    {
        [JsonProperty]
        public string AccountId { get; set; }

        [JsonProperty]
        public decimal Balance { get; set; }
    }
}
