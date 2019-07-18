namespace TinyMock
{
    public class MockModel
    {
        public string Id { get; set; }
        public ActionType Action { get; set; }
        public object Scheme { get; set; }

        public static ActionType MapToActionType(string action)
        {
            switch (action)
            {
                case "add":
                case "create":
                    return ActionType.Create;
                case "edit":
                case "update":
                case "move":
                    return ActionType.Edit;
                case "remove":
                case "delete":
                    return ActionType.Delete;
                case "clean":
                case "clear":
                    return ActionType.Clear;
            }
            return ActionType.Undefined;
        }
    }

    public enum ActionType
    {
        Undefined,
        Create,
        Edit,
        Delete,
        Clear
    }
}