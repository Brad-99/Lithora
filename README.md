📘 Lithora – Lithography Quality Tracking Platform
Overview

Lithora is an internal web-based quality tracking platform designed for lithography process inspections.
It enables engineers to record inspection results, track defects, analyze quality trends, and support manufacturing decisions.

Core Features

Inspection & Defect management

Machine data simulation for testing

Quality statistics (fail rate, trends)

RESTful API design

Pluggable AI defect analysis (extensible)

Architecture
Frontend (TypeScript)
|
v
ASP.NET Core Controllers
|
v
Services (Business Logic)
|
v
Entity Framework Core
|
v
SQLite Database

Technology Stack

Backend: ASP.NET Core Web API (.NET)

Database: SQLite + EF Core

Frontend: TypeScript

Testing: xUnit

AI: Interface-based integration (Mock / Future LLM)

Why Lithora

Lithora focuses on system design, data flow, and maintainability rather than hard-coded process logic, making it adaptable to real manufacturing environments.

Lithora 是我設計的一個製程品質追蹤系統，目標是模擬真實製造 IT 系統如何處理檢測資料、缺陷紀錄與品質分析。
我採用 ASP.NET Core Web API，並以 Controller–Service–Data 的分層架構設計，確保業務邏輯與資料存取解耦。
由於我沒有實體機台，我另外設計了一個 machine simulator 來模擬 Pass/Fail 與 defect 行為，讓系統能在沒有設備的情況下進行測試。
在設計上我也預留了 AI 擴充介面，用於未來做 defect 分類與初步原因分析，但不耦合在核心系統中。
這個專案讓我完整練習了 .NET、RESTful API、SQL、以及如何設計可維運的製造 IT 系統。

http://localhost:5157/swagger/
