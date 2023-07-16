namespace CorrelationId.Middlewares;

public interface ICorrelationIdManager {
  string Get();
  void Set(string correlationString);
}
