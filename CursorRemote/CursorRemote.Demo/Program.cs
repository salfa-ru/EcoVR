
CursorRemote.Library.CursorApi api = new CursorRemote.Library.CursorApi();
Console.WriteLine("Выделение области");
api.MoveRelative(0.0D, 0.2D);
api.SetMouseLeftDown();
Thread.Sleep(1000);
api.MoveRelative(0.4D, 0.5D);
Thread.Sleep(1000);
api.SetMouseLeftUp();


Thread.Sleep(1000);
Console.WriteLine("Скролл");
api.Move(10, 300);
api.SetMouseLeftClick();
Thread.Sleep(1000);
api.SetMouseUpScroll(1000);
Thread.Sleep(1000);
api.SetMouseDownScroll(1000);
Thread.Sleep(1000);
api.SetMouseRightClick();
