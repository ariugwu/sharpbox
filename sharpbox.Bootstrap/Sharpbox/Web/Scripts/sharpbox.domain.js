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
        var TestController;
        (function (TestController) {
            var Command = (function () {
                function Command() {
                    throw new Error("Cannot new this class");
                }
                Command.TestCommand = "TestCommand";
                Command.SaveExampleModel = "SaveExampleModel";
                return Command;
            })();
            TestController.Command = Command;
            var Event = (function () {
                function Event() {
                    throw new Error("Cannot new this class");
                }
                Event.TestEvent = "TestEvent";
                return Event;
            })();
            TestController.Event = Event;
            var UiAction = (function () {
                function UiAction() {
                    throw new Error("Cannot new this class");
                }
                UiAction.CouldBeAnything = "CouldBeAnything";
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
var sharpbox;
(function (sharpbox) {
    var Domain;
    (function (Domain) {
        var EnvironmentController;
        (function (EnvironmentController) {
            var Command = (function () {
                function Command() {
                    throw new Error("Cannot new this class");
                }
                return Command;
            })();
            EnvironmentController.Command = Command;
            var Event = (function () {
                function Event() {
                    throw new Error("Cannot new this class");
                }
                return Event;
            })();
            EnvironmentController.Event = Event;
            var UiAction = (function () {
                function UiAction() {
                    throw new Error("Cannot new this class");
                }
                return UiAction;
            })();
            EnvironmentController.UiAction = UiAction;
            var Routine = (function () {
                function Routine() {
                    throw new Error("Cannot new this class");
                }
                return Routine;
            })();
            EnvironmentController.Routine = Routine;
        })(EnvironmentController = Domain.EnvironmentController || (Domain.EnvironmentController = {}));
    })(Domain = sharpbox.Domain || (sharpbox.Domain = {}));
})(sharpbox || (sharpbox = {}));
