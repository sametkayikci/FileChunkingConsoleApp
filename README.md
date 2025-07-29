# 🧩 File Chunking Console Application

Bu proje, büyük dosyaları küçük parçalara (chunk) ayıran, parçaları çeşitli depolama sağlayıcılarına yükleyen ve daha sonra bu parçaları tekrar birleştiren modüler bir .NET 9 tabanlı **konsol uygulamasıdır**.

---

## 📦 Proje Yapısı

```
FileChunkingConsoleApp/
├── Application/
│   ├── Helpers/
│   │   └── ChecksumHelper.cs
│   ├── Interfaces/
│   │   └── IMetadataRepository.cs
│   ├── Services/
│   │   └── FileProcessorService.cs
│   └── GlobalUsings.cs
│
├── ConsoleApp/
│   ├── Cli/
│   │   ├── CliExecutor.cs
│   │   ├── CliOptions.cs
│   │   ├── CliOptionsBinder.cs
│   │   └── CliOptionsValidator.cs
│   ├── appsettings.json
│   ├── Program.cs
│   └── GlobalUsings.cs
│
├── Domain/
│   ├── Entities/
│   │   ├── ChunkMetadata.cs
│   │   └── FileMetadata.cs
│   ├── Enums/
│   │   └── ChunkStrategyType.cs
│   └── Interfaces/
│       ├── IChunkStrategy.cs
│       ├── IChunkStrategyFactory.cs
│       ├── IFileProcessor.cs
│       └── IStorageProvider.cs
│       └── GlobalUsings.cs
│
├── Infrastructure/
│   ├── ChunkStrategies/
│   │   ├── ChunkStrategyFactory.cs
│   │   ├── DynamicChunkStrategy.cs
│   │   ├── FixedSizeChunkStrategy.cs
│   │   └── TargetChunkCountStrategy.cs
│   │
│   ├── Db/
│   │   ├── ChunkMetadataContext.cs
│   │   └── Configurations/
│   │       ├── ChunkMetadataConfiguration.cs
│   │       └── FileMetadataConfiguration.cs
│   │
│   ├── Entities/
│   │   └── ChunkBlobEntity.cs
│   │
│   ├── Extensions/
│   │   ├── FileInfoExtensions.cs
│   │   └── ServiceCollectionExtensions.cs
│   │
│   ├── Repositories/
│   │   └── MetadataRepository.cs
│   │
│   ├── Services/
│   │   └── ChunkGenerator.cs
│   │
│   ├── StorageProviders/
│   │   ├── DatabaseStorageProvider.cs
│   │   └── FileSystemStorageProvider.cs
│   │
│   └── GlobalUsings.cs
│
├── FileChunkingConsoleApp.Tests/
│   ├── ChecksumHelperTests.cs
│   ├── CliOptionsValidatorTests.cs
│   ├── FileInfoExtensionsTests.cs
│   ├── FileProcessorServiceTests.cs
│   └── GlobalUsings.cs
│
└── Solution Items/
    └── file_chunking_console_app_solution.md


```

---

## ✨ Özellikler

- ✅ Dosyaları belirli boyutlarda veya hedef chunk sayısına göre bölebilme
- ✅ Chunk'ları `FileSystem` veya `Database` gibi sağlayıcılara yükleme
- ✅ Metadata bilgilerini veritabanında (PostgreSQL) saklama
- ✅ SHA-256 checksum ile bütünlük doğrulama
- ✅ CLI üzerinden esnek yönetim (process / merge / strategy)
- ✅ FluentValidation ile CLI doğrulama
- ✅ Genişletilebilir mimari: yeni storage provider, chunking strategy kolay eklenebilir
- ✅ Detaylı `ILogger` loglama

---

## 🧱 Mimari Yapı

Bu uygulama **Clean Architecture + SOLID** prensiplerine uygun şekilde geliştirilmiştir.

### 🔹 Domain
- İş kurallarını ve modelleri içerir (`ChunkMetadata`, `FileMetadata`, `IFileProcessor`, vs.)

### 🔹 Application
- İş akışlarını ve servisleri içerir (`FileProcessorService`)
- `CliExecutor` sınıfı uygulama başlatıcı olarak çalışır.

### 🔹 Infrastructure
- Gerçekleştirme (implementation) katmanıdır.
- Veritabanı, stratejiler, storage provider’lar burada bulunur.

### 🔹 CLI
- `System.CommandLine` ile tüm argümanlar dış dünyadan alınır.
- `CliOptionsBinder` ile POCO'ya bağlanır, `CliOptionsValidator` ile doğrulanır.

### 🎨 Kullanılan Design Pattern’ler

| Pattern                  | Açıklama |
|--------------------------|----------|
| **Strategy Pattern**     | `DynamicChunkStrategy`, `FixedSizeChunkStrategy`, `TargetChunkCountStrategy` gibi algoritmalar `IChunkStrategy` arayüzü ile çalışır. |
| **Factory Pattern**      | `ChunkStrategyFactory`, `IChunkStrategy` implementasyonlarını dinamik üretir. |
| **Dependency Injection** | `ServiceCollectionExtensions` ile bağımlılıklar dışarıdan enjekte edilir. |
| **Repository Pattern**   | Veritabanı işlemleri `IMetadataRepository` ve `MetadataRepository` ile soyutlanmıştır. |
| **Command Pattern**      | `CliExecutor`, CLI komutlarını uygulama mantığına çevirir. |
| **Validation Pattern**   | `CliOptionsValidator` ile `FluentValidation` üzerinden doğrulama yapılır. |
| **Extension Methods**    | `FileInfoExtensions.NameHash()` gibi metotlar mevcut sınıfları genişletir. |
| **Builder Pattern**      | `ServiceCollectionExtensions.AddFileChunking()` yapılandırmaları zincirleme ve okunabilir hale getirir. |


---

## 🚀 CLI Kullanımı

### 🔧 Kurulum

```bash
dotnet build
dotnet test
```

### 📂 Dosya Parçalama (chunking)

```bash
dotnet run --project ConsoleApp -- \
  --process ./bigfile.zip \
  --strategy Fixed
```

### 🧩 Parçaları Birleştirme (merge)

```bash
dotnet run --project ConsoleApp -- \
  --merge 23e4aa8d-ff33-49e1-84d1-9b06f712c113 \
  --output ./merged-output.zip
```

### 🔍 Yardım Almak

```bash
dotnet run -- --help
```

---

## ⚙️ `appsettings.json` Örneği

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Database=chunks;Username=postgres;Password=yourpass"
  },
  "Chunking": {
    "MaxChunkSize": 5242880,
    "FixedSize": 10485760,
    "TargetCount": 10
  }
}
```

---

## 🛠️ Genişletilebilirlik

| Bileşen               | Nasıl Eklenir? |
|----------------------|----------------|
| Yeni `IStorageProvider` | `StorageProviders/` klasörüne ekle ve DI container’a kaydet |
| Yeni `IChunkStrategy`   | `ChunkStrategies/` klasörüne ekle ve `ChunkStrategyFactory`'de tanımla |
| Yeni CLI parametresi   | `CliOptions.cs` ve `CliOptionsBinder.cs`'ye ekle |

---

## 🧪 Test & Geliştirme

```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project ConsoleApp
dotnet ef database update -p Infrastructure -s ConsoleApp
```

> Uygulama PostgreSQL ile çalışır, `ChunkMetadataContext` yapılandırması hazırdır.

---

## 🧾 Lisans

MIT License. Bu projeyi özgürce kullanabilir, genişletebilirsiniz.

---

## ✍ Katkıda Bulun

Her türlü öneri, düzeltme ve katkı PR olarak gönderilebilir.  
Ayrıca `README.md` içindeki örneklerin daha da geliştirilebilmesi için katkıya açıktır.