# PaladinHub.Tests

Тестов пакет за PaladinHub, базиран на NUnit + Moq + EF Core InMemory.

## Стартиране

1. Отвори solution-а на проекта и добави този тестов проект.
2. Увери се, че PaladinHub е таргетиран към net8.0 (както тестовете).
3. Инсталирай пакетите (ако не се възстановят автоматично).
4. Пусни тестовете:
   ```bash
   dotnet test PaladinHub.Tests/PaladinHub.Tests.csproj
