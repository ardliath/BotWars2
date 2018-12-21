namespace BotWars2Server.Code.Communication
{
    public interface ICommander
    {
        void Register(RegisterData data);
        void Turn(TurnData data);
    }
}