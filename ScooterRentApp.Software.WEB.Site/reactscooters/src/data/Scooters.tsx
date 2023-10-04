type Scooter = {
    mac: string,
    speed: number,
    position: Position,
    rentalTime: number,
    batteryLevel: number
}

type Position = {
    latitude: number,
    longitude : number
}

type ScooterProperty = {
    mac : string,
    property : PropertyTypes,
    value : any
  }
  
  enum PropertyTypes
  {
    BateryLevel,
    RentalTime,
    Speed,
    Position,
    MAC
  }