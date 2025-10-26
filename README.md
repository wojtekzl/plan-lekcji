# 🗓️ Plan Lekcji – Aplikacja WPF do zarządzania harmonogramem zajęć

---

## 📌 Opis projektu

**Plan Lekcji** to aplikacja desktopowa stworzona w technologii **WPF (.NET)**, służąca do przedstawiania planu lekcji w formie siatki tabelarycznej. Projekt umożliwia przejrzyste wyświetlenie godzin lekcyjnych i dni tygodnia, a także stanowi świetną bazę do dalszej rozbudowy funkcjonalności, takich jak edycja planu, zapis danych czy eksport.

---

## 🧠 Cele projektu

✔ Uporządkowana prezentacja harmonogramu  
✔ Nauka praktycznego użycia XAML i WPF  
✔ Możliwość rozbudowy (zapis danych, interaktywność)  
✔ Przystępna forma dla uczniów, nauczycieli i rodziców  

---

## 🛠️ Technologie

| Narzędzie       | Wersja / Uwagi                      |
|------------------|--------------------------------------|
| **Język**        | C#                                  |
| **Framework**    | .NET 8.0                            |
| **UI**           | Windows Presentation Foundation (WPF) |
| **XAML**         | Definicja interfejsu użytkownika     |
| **IDE**          | Visual Studio 2022                   |

---

## ⚙️ Proces tworzenia (krok po kroku)

### 1️⃣ Inicjalizacja projektu

- Nowy projekt typu `WPF App (.NET)`
- Konfiguracja okna (`MainWindow.xaml`):
  - `Title="Plan Lekcji"`
  - `Width="970"`, `Height="790"`
  - `WindowStartupLocation="CenterScreen"`

### 2️⃣ Struktura `Grid`

- Główna siatka podzielona na:
  - **7 kolumn**: numeracja, godziny, dni tygodnia (Pon–Pt)
  - **12 wierszy**: nagłówki + 11 godzin lekcyjnych

### 3️⃣ Dodanie nagłówków dni tygodnia

- Wiersz 0: `Poniedziałek`, `Wtorek`, `Środa`, `Czwartek`, `Piątek`
- Każda komórka to `Border` z `TextBlock`

### 4️⃣ Numeracja lekcji

- Kolumna 0, wiersze 1–11: `1.` do `11.`

### 5️⃣ Godziny lekcyjne

- Kolumna 1, wiersze 1–11 z przedziałami czasowymi

### 6️⃣ Stylizacja i wygląd

- Marginesy, obramowania (`BorderBrush="Black"`, `Margin="1"`)
- Wyrównanie tekstu (`HorizontalAlignment="Center"`, `VerticalAlignment="Center"`)
- Pogrubienie (`FontWeight="Bold"`)

---

## ⏰ Godziny lekcyjne

| Nr | Przedział czasowy     |
|----|------------------------|
| 1  | 7:10 – 7:55            |
| 2  | 8:00 – 8:45            |
| 3  | 8:50 – 9:35            |
| 4  | 9:40 – 10:25           |
| 5  | 10:35 – 11:20          |
| 6  | 11:30 – 12:15          |
| 7  | 12:30 – 13:15          |
| 8  | 13:25 – 14:10          |
| 9  | 14:20 – 15:05          |
| 10 | 15:10 – 15:55          |
| 11 | 16:00 – 16:45          |

---

## 📅 Dni tygodnia

- Poniedziałek  
- Wtorek  
- Środa  
- Czwartek  
- Piątek  

---

## 💾 Zapis danych (planowana funkcjonalność)

plik plan_lekcji.txt zapisuje wszystkie dane przez co nie trzeba mieć internetu aby zapamiętać zmiany

---

## 📌 Informacje końcowe

- Autor Wojciech Złonkiewicz  
- Opis Projek planu lekcji na 6  
- Źródła Github, wiedza własnam, Youtube, ChatGPT  
- Czwartek  
- Piątek  

---

## 📜 Licencja i autor
Projekt udostępniony na licencji MIT License – możesz go dowolnie modyfikować i używać w projektach prywatnych lub komercyjnych.

Autorem projektu jest Maciej Strzelec.

Projekt wykonano w Sierpniu 2025

---
