# sharpbox
A group of common intranet application components (modules) that are encapsulated within a (app) context and communicate via a dispatcher. The dispatcher and the concept of a unidirectional data flow come from the [Facebook Flux Architecture](http://facebook.github.io/flux/docs/overview.html).

Where the simplicity of that approach fails the concepts of [Event Sourcing] (https://msdn.microsoft.com/en-us/library/jj591559.aspx), [CQRS] (https://msdn.microsoft.com/en-us/library/dn568103.aspx) and [Domain Driven Design] (https://domainlanguage.com/ddd/) will come into play. For example an aggregate root is a great candidate for a domain. (i.e - In a Cookbook app the 'Reciepe' would be the aggregate root of Ingredients, Steps, and Notes associated with that entity). However the possible business questions (queries) related to receipes (i.e. - GetAllReceipesWhichIncludeCheese()) should be developed and extended separately from the command domain. So we have a command domain and a query domain (CQRS).

There's a low ceiling on the scope and features for this approach. Nested commands and replay functionality are not within scope [for example]. There's some 'better than what we have today' goal that lies between **_but can it do this_** and **_inscrutable_**.

## The problem it solves:

#### Turn this
```c#
public static T SomeUnitOfWork(T SomeObject, DependencyA depA, DependencyB depB){
    // Do Stuff to Object
    someObject.DoStuffExtension();
    // Run a check
    if(someObject.ResultOfExtension > 5){
      // Do Other stuff.
    }
    
    // Call the first dependency to do something like send email.
    depA.UpdateWithStatusOfSomeObject(someObject);
    
    // Call the second dependency for whatever reason
    someObject = depB.PersistObject(someObject);
    
    return SomeObject;
}

static void Main(string[] args){
  var something = new SomeObject();
  var depA = new DependencyA();
  var debB = new DependencyB();
  var logger = new Logger();
  
  debB.ObjectPersists += logger.LogEvent;
  
  something = SomeUnitOfWork(something, depA, depB);
}
```

#### Into this

```c#
public static T SomeUnitOfWork(T someObject){
    // Do Stuff to Object
    someObject.DoStuffExtension();
    return someObject;
}

static void Main(string[] args){
  var dispatch = new Dispatch();
  var depA = new DependencyA();
  var debB = new DependencyB();
  var logger = new Logger();
  var someObject = new SomeObject();
  var container = EncapsulateComponents(dispatch, depA, debB, logger, someObject);
  
  // Says: When you process 'FireSomeUnitOfWork' call 'SomeUnitOfWork' then pass the result to 'OnStuffDone' and broadcast to listeners.
  dispatch.Register(FireSomeUnitOfWork, SomeUnitOfWork, OnStuffDone);
  
  // Once the SomeUnitOfWork fires it will broadcast to anyone listening. Below we'll register some listeners.
  dispatch.Listen(OnStuffDone, container.UpdateSomethingObject);
  dispatch.Listen(OnStuffDone, container.depA.UpdateUsersWithStatusOfSomeObject);
  dispatch.Listen(OnStuffDone, container.depB.PersistObject);
  dispatch.Listen(OnStuffDone, container.app.Logger.LogEvent);
  
  // Send a request to process the object in our container.
  dispatch.Process(FireSomeUnitOfWork, container.Something);
  
  // The command stream contains all of our processed actions as well as their pre and post processed states.
  var commandStream = dispatch.CommandStream; 
}
```

The benefit (hopefully) is that we can maintain single responsbility throughout the system. At the same time we can have clean central auditing and extend the system from the calling layer instead of nesting functionality in business logic. 

Keep in the mind that the first example gets *more* complicated in a real example as you add error handling, roll backs, and such. The second example gets *less* complicated as each channel is self contained.
