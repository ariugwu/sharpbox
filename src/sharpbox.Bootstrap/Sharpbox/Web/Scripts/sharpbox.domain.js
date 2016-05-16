var sharpbox;
(function (sharpbox) {
    var Domain;
    (function (Domain) {
        var ExampleChildController;
        (function (ExampleChildController) {
            var Command = (function () {
                function Command() {
                    throw new Error("Cannot new this class");
                }
                return Command;
            })();
            ExampleChildController.Command = Command;
            var Event = (function () {
                function Event() {
                    throw new Error("Cannot new this class");
                }
                return Event;
            })();
            ExampleChildController.Event = Event;
            var UiAction = (function () {
                function UiAction() {
                    throw new Error("Cannot new this class");
                }
                return UiAction;
            })();
            ExampleChildController.UiAction = UiAction;
            var Routine = (function () {
                function Routine() {
                    throw new Error("Cannot new this class");
                }
                return Routine;
            })();
            ExampleChildController.Routine = Routine;
        })(ExampleChildController = Domain.ExampleChildController || (Domain.ExampleChildController = {}));
    })(Domain = sharpbox.Domain || (sharpbox.Domain = {}));
})(sharpbox || (sharpbox = {}));
var sharpbox;
(function (sharpbox) {
    var Domain;
    (function (Domain) {
        var HomeController;
        (function (HomeController) {
            var Command = (function () {
                function Command() {
                    throw new Error("Cannot new this class");
                }
                return Command;
            })();
            HomeController.Command = Command;
            var Event = (function () {
                function Event() {
                    throw new Error("Cannot new this class");
                }
                return Event;
            })();
            HomeController.Event = Event;
            var UiAction = (function () {
                function UiAction() {
                    throw new Error("Cannot new this class");
                }
                return UiAction;
            })();
            HomeController.UiAction = UiAction;
            var Routine = (function () {
                function Routine() {
                    throw new Error("Cannot new this class");
                }
                return Routine;
            })();
            HomeController.Routine = Routine;
        })(HomeController = Domain.HomeController || (Domain.HomeController = {}));
    })(Domain = sharpbox.Domain || (sharpbox.Domain = {}));
})(sharpbox || (sharpbox = {}));
var sharpbox;
(function (sharpbox) {
    var Domain;
    (function (Domain) {
        var ExampleModelController;
        (function (ExampleModelController) {
            var Command = (function () {
                function Command() {
                    throw new Error("Cannot new this class");
                }
                Command.TestCommand = "TestCommand";
                Command.SaveExampleModel = "SaveExampleModel";
                return Command;
            })();
            ExampleModelController.Command = Command;
            var Event = (function () {
                function Event() {
                    throw new Error("Cannot new this class");
                }
                Event.TestEvent = "TestEvent";
                return Event;
            })();
            ExampleModelController.Event = Event;
            var UiAction = (function () {
                function UiAction() {
                    throw new Error("Cannot new this class");
                }
                UiAction.CouldBeAnything = "CouldBeAnything";
                return UiAction;
            })();
            ExampleModelController.UiAction = UiAction;
            var Routine = (function () {
                function Routine() {
                    throw new Error("Cannot new this class");
                }
                return Routine;
            })();
            ExampleModelController.Routine = Routine;
        })(ExampleModelController = Domain.ExampleModelController || (Domain.ExampleModelController = {}));
    })(Domain = sharpbox.Domain || (sharpbox.Domain = {}));
})(sharpbox || (sharpbox = {}));
//# sourceMappingURL=sharpbox.domain.js.map