using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Software.Server.Hubs
{
    public class ScooterProperty
    {
        public string Mac {  get; set; }
        public RecieveProperty property {  get; set; } 
        public dynamic Value { get; set; }

        public ScooterProperty() { }
        public ScooterProperty(string mac, RecieveProperty prop, dynamic val)
        {
            Mac = mac;
            property = prop;
            Value = val;
        }
    }
}
