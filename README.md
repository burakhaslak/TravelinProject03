# 🌍 Travelin — Asp.Net Core Tour Booking Project

Travelin is a comprehensive dynamic tour booking system built with **Asp.Net Core 6.0** using **MVC Architecture** with **MongoDB** as the database. The project covers modern web development patterns, clean service layer architecture, email integrations, and a fully featured admin panel.

---

## 🚀 Highlights

### 🗂️ Admin Panel
A fully custom admin panel built with a consistent design system (card-based UI, blue `#2563eb` primary color). The panel includes:
- **Dashboard** with real-time stats: total tours, bookings, revenue, and review averages
- **Booking management** with Approve / Cancel actions
- **Review management** with moderation support
- **Report generation** — export Booking, Revenue, and Tour data as **Excel (.xlsx)** or **PDF**

### 📅 Booking & Email System
- Customers can book tours directly from the tour detail page
- On approval or cancellation, an automated **HTML email** is sent to the customer via **MailKit / MimeKit**
- Email content includes tour name, booking date, and customer name

### ⭐ Comment & Review System
- Only customers with an **approved booking** for a tour can leave a review
- Reviews include a star rating (1–5), headline, and comment detail
- Average score and review count are displayed on the tour detail and tour listing pages
- Admin can moderate (approve/delete/edit) reviews from the panel

### 📊 Reporting
- Export **Booking Report** → all reservations with status, pricing, and customer info
- Export **Revenue Report** → revenue by tour and monthly breakdown
- Export **Tour Report** → all tours with location, capacity, and date info
- All reports available in both **Excel** (multi-sheet, color-coded) and **PDF** (landscape, styled tables) formats

### 🗺️ Tour Categorization
- Tours are categorized as **Domestic / International** and **Visa Required / Visa Free**
- Dedicated pages for `No Visa Tours` and `Domestic Tours` accessible from the main navigation

### 🌍 Multi-Language Support

The platform supports 6 languages via Google Translate widget:
English, German, Arabic, French, Turkish, Russian

---

## 🛠️ Tech Stack & Key Features

| Layer | Technology |
|---|---|
| **Backend** | Asp.Net Core 6.0, C# |
| **Database** | MongoDB (NoSQL) |
| **Architecture** | MVC, Service Layer, Repository Pattern |
| **Email** | MailKit, MimeKit (Gmail SMTP) |
| **Mapping** | AutoMapper |
| **Excel Export** | ClosedXML |
| **PDF Export** | iTextSharp |
| **UI** | Bootstrap 5, Bootstrap Icons, Font Awesome |
| **Frontend** | Razor Views, ViewComponents, Partial Views |
| **Localization** | Google Translate Widget (EN, DE, AR, FR, TR, RU) |

---

## ✨ Features at a Glance

- 🏠 Dynamic homepage with featured tours and categories
- 📋 Tour listing with pagination, average ratings, and review counts
- 🔍 Tour detail page with daily plan, map, booking form, and customer reviews
- ✅ Booking approval / cancellation with automated email notifications
- 💬 Review system with booking verification gate
- 🗺️ No Visa Tours & Domestic Tours dedicated pages
- 📈 Admin dashboard with live statistics and booking status breakdown
- 📁 Excel & PDF report exports
- 🌐 English culture default for consistent date/number formatting

---
### Credits
This project was developed as part of the Asp.Net Core 6.0 Project Camp by **Murat Yücedağ**.

---

# 📸 Screenshots

## HOME & TOUR LIST & NO VISA TOURS & DOMESTIC TOURS
![Homepage](https://github.com/user-attachments/assets/b654f4f0-c81b-4f96-87ee-6b46effd4441)
![localhost_7122_Tour_TourList_](https://github.com/user-attachments/assets/3292196e-f85e-451e-8f3c-f3cdbeef7b14)
![localhost_7122_Tour_DomesticTourList_](https://github.com/user-attachments/assets/d828fbab-b534-45de-980b-5c06ccd4167e)
![localhost_7122_Tour_VisaFreeTourList_](https://github.com/user-attachments/assets/bdb7d10b-99b7-4892-ac96-3cb96d79b2b3)

## TOUR DETAILS
![localhost_7122_Tour_TourDetails_69af3def35e2b479994f3fd7](https://github.com/user-attachments/assets/c571495e-e3fa-4ecb-8d71-88b7bce5d1a2)


## BOOKING
<img width="1900" height="945" alt="image" src="https://github.com/user-attachments/assets/e615b104-504e-42a7-93a8-ad7175fa12b0" />
<img width="1903" height="946" alt="image" src="https://github.com/user-attachments/assets/0103b67d-d937-4871-ae80-4ad0dde09f3d" />
<img width="1905" height="950" alt="image" src="https://github.com/user-attachments/assets/4f8d5687-7248-4677-9b54-4f3ea9e6d137" />


## ADMIN PANEL

### Dashboard
<img width="2880" height="2394" alt="localhost_7122_AdminDashboard_DashboardPage_" src="https://github.com/user-attachments/assets/08b2593f-9237-42c5-8c87-105d9b01a3cc" />

### Tour Lists
<img width="1908" height="952" alt="image" src="https://github.com/user-attachments/assets/9ecc98ec-8ad8-4b27-88cd-c656d948b87a" />
<img width="2880" height="5226" alt="localhost_7122_AdminTour_CreateTour_" src="https://github.com/user-attachments/assets/3cb7c5ab-9b4d-4ab7-a78b-e23662c7741f" />
<img width="1920" height="946" alt="image" src="https://github.com/user-attachments/assets/33e906e9-5136-44f3-8264-8e41527dc19a" />

### Tour Plans
<img width="1909" height="951" alt="image" src="https://github.com/user-attachments/assets/f7775e8c-3e8b-44fd-bb0e-a2198c8a9b08" />
<img width="1917" height="950" alt="image" src="https://github.com/user-attachments/assets/5acf643f-981d-439a-9c7f-cd519858fc2c" />

### Booking List
<img width="1902" height="951" alt="image" src="https://github.com/user-attachments/assets/147b9378-bcdf-483c-8c37-2d7d0c4dbf57" />
<img width="724" height="459" alt="image" src="https://github.com/user-attachments/assets/0ccfb2a2-d2d6-4b31-b8a3-cee31399abfb" />
<img width="724" height="503" alt="image" src="https://github.com/user-attachments/assets/a5c8165c-b6c8-45b7-b673-76c5b79ba5da" />

### Report Page
<img width="1917" height="951" alt="image" src="https://github.com/user-attachments/assets/bfe099af-345f-4dd0-8350-09c008d591aa" />
<img width="1324" height="938" alt="image" src="https://github.com/user-attachments/assets/1d863795-c96e-45ca-8e04-4d8725c38158" />
<img width="928" height="239" alt="image" src="https://github.com/user-attachments/assets/66771365-cf1d-46f4-a87b-1f89962f0232" />

### Review List
<img width="1906" height="954" alt="image" src="https://github.com/user-attachments/assets/9c2cda26-4934-47d2-8c8f-ddc8ae098679" />
<img width="1912" height="949" alt="image" src="https://github.com/user-attachments/assets/aff7a154-5ad7-497d-8f80-42721e61ab19" />







