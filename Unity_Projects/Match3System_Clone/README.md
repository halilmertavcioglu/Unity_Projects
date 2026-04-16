## 🛠🔵 Techniques Used in Match 3

- Singleton Pattern Implementation: Centralized game management for global state persistence and seamless communication between game logic and UI.
- Grid-Based Logic & 2D Coordinate Mapping: Managing 2D arrays to track, move, and validate object positions on a dynamic board.
- Recursive Match Detection Algorithm: Implementing recursive logic to identify and group identical adjacent elements for complex matching patterns.
- State Machine Architecture: Managing distinct game phases (Input, Swapping, Matching, Falling) to prevent logic conflicts during gameplay.
- ScriptableObjects for Data Architecture: Using ScriptableObjects to decouple game data (gem types, level goals) from the logic, allowing easy scalability.
- Dynamic Board Refilling Logic: A custom algorithm to calculate empty spaces and spawn new elements in the correct sequence.

## 🎮 Gameplay Video
[You can access the gameplay video via this link (Google Drive).](https://drive.google.com/file/d/1_IFIwuv_DCQCDCIMj33YKgEcWMptU2uv/view?usp=sharing)

## ⚠️ Note
This project is a personal educational clone inspired by the original game. It is developed for learning purposes only and not for commercial use.


---

## 🛠🔵 Match 3 Oyununda Kullanılan Teknikler

- Singleton Pattern Uygulaması: Oyun durumunun korunması ve oyun mantığı ile arayüz (UI) arasındaki iletişimi kolaylaştıran merkezi yönetim yapısı.
- Izgara Mantığı ve 2D Koordinat Eşleştirme: Dinamik tahta üzerindeki nesne konumlarını takip etmek, taşımak ve doğrulamak için 2D dizi yönetimi.
- Özyinelemeli (Recursive) Eşleşme Algoritması: Karmaşık eşleşme desenlerini tespit etmek ve komşu öğeleri gruplamak için özyinelemeli mantık kurgusu.
- Durum Makinesi (State Machine) Mimarisi: Oyun akışındaki çakışmaları önlemek için farklı aşamaların (Giriş, Takas, Eşleşme, Düşme) birbirinden bağımsız yönetimi.
- Veri Mimarisi için ScriptableObject Kullanımı: Oyun verilerini (taş türleri, seviye hedefleri) mantıktan ayırarak projenin ölçeklenebilirliğini artırma.
- Dinamik Tahta Yenileme Mantığı: Boşlukları hesaplayan ve yeni öğelerin doğru sırayla tahtaya dahil olmasını sağlayan özel algoritma.

## 🎮 Oynanış Videosu
[Oynanış videosuna bu linkten ulaşabilirsiniz (Google Drive).](https://drive.google.com/file/d/1_IFIwuv_DCQCDCIMj33YKgEcWMptU2uv/view?usp=sharing)

## ⚠️ Not
Bu proje, orijinal oyundan esinlenerek eğitim amaçlı geliştirilmiş bir klon çalışmadır. Ticari amaç taşımamaktadır.
