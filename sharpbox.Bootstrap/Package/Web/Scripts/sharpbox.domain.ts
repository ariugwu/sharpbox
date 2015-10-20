


 


module sharpbox.Domain.TestController {
export class Command {
constructor() {throw new Error("Cannot new this class");}
static TestCommand = "TestCommand";
static SaveExampleModel = "SaveExampleModel";
static Add = "Add";
static Update = "Update";
static Remove = "Remove";
}

export class Event {
constructor() {throw new Error("Cannot new this class");}
static TestEvent = "TestEvent";
static OnAdd = "OnAdd";
static OnUpdate = "OnUpdate";
static OnRemove = "OnRemove";
}

export class UiAction {
constructor() {throw new Error("Cannot new this class");}
}

export class Routine {
constructor() {throw new Error("Cannot new this class");}
}

}
