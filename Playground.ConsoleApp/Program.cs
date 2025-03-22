using Playground.ConsoleApp.TypingGame;
using Playground.ConsoleApp.TypingGame.Screens.Main;

//DemoConsoleApplication.Create().Run<DemoEndingScreen>(new DemoStartingScreen());

var app = TypingGameConsoleApplication.Create();

app.Run<ClosingScreen>(new OpeningScreen(app));