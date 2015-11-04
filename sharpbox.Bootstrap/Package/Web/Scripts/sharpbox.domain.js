var sharpbox;
(function (sharpbox) {
    var Domain;
    (function (Domain) {
        var TestController;
        (function (TestController) {
            var Command = (function () {
                function Command() {
                    throw new Error("Cannot new this class");
                }
                Command.TestCommand = "TestCommand";
                Command.SaveExampleModel = "SaveExampleModel";
                Command.Add = "Add";
                Command.Update = "Update";
                Command.Remove = "Remove";
                return Command;
            })();
            TestController.Command = Command;
            var Event = (function () {
                function Event() {
                    throw new Error("Cannot new this class");
                }
                Event.TestEvent = "TestEvent";
                Event.OnAdd = "OnAdd";
                Event.OnUpdate = "OnUpdate";
                Event.OnRemove = "OnRemove";
                return Event;
            })();
            TestController.Event = Event;
            var UiAction = (function () {
                function UiAction() {
                    throw new Error("Cannot new this class");
                }
                return UiAction;
            })();
            TestController.UiAction = UiAction;
            var Routine = (function () {
                function Routine() {
                    throw new Error("Cannot new this class");
                }
                return Routine;
            })();
            TestController.Routine = Routine;
        })(TestController = Domain.TestController || (Domain.TestController = {}));
    })(Domain = sharpbox.Domain || (sharpbox.Domain = {}));
})(sharpbox || (sharpbox = {}));
