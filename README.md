---
project: Tayana Yacht
category: Portfolio Project (Back-end)
version: 1.0.0
summary: A yacht brand website with an admin system for managing yachts, news, dealers and contact messages.
features: Soft delete, publish rules, privacy policy versioning
developer: unclehank5255
---

# Tayana Yacht 遊艇品牌網站與後台管理系統

火箭隊培訓營 23 梯後端專題

> **免責聲明**
> 本專案為個人練習作品，僅供學習與作品集展示使用，不作任何商業用途。本專案並非 Tayana Yachts 官方網站，與該品牌無任何商業合作或授權關係。網站中使用的品牌名稱、商標、圖片及文字內容，版權皆屬原權利人所有。

## 簡介

這是一個用 ASP.NET MVC 5 做的遊艇品牌網站，分成前台和後台。

- 前台：瀏覽遊艇、最新消息、經銷商，並可以填寫聯絡表單
- 後台：管理員登入後，可以管理網站上的文字、圖片和 PDF 檔案

## 畫面截圖

<img width="100%" alt="前台首頁" src="https://github.com/user-attachments/assets/afb8ffc4-b8df-451c-82ab-e3e8cc345b02" />
<img width="100%" alt="後台遊艇管理" src="https://github.com/user-attachments/assets/be4d1c92-96d3-4e00-a4e7-5fee8dd01f7f" />

## 專案亮點

- 使用 ASP.NET MVC 5 建立前台網站與後台內容管理平台
- 以 Entity Framework 6 Code First 設計關聯資料模型，用 Migrations 管理資料庫變更
- 實作 Forms Authentication 後台登入，密碼以 Crypto 雜湊後儲存
- 整合 CKEditor 與 elFinder，支援內容編輯、圖片與 PDF 上傳，並在伺服器端檢查上傳路徑
- 支援內容上架 / 下架、軟刪除與排序，必填內容未完成時無法上架
- 隱私權條款版本管理，聯絡表單留言會記錄訪客同意的版本

## 使用技術

- ASP.NET Framework 4.8、ASP.NET MVC 5、C#
- Entity Framework 6 Code First、SQL Server
- Razor Views、Bootstrap 5、jQuery
- 後台模板：AdminLTE
- 後台內容編輯工具：CKEditor
- 檔案管理工具：elFinder（elFinder.AspNet）
- 驗證：Forms Authentication（後台）、Anti-forgery token
- 密碼雜湊：System.Web.Helpers Crypto（HashPassword / VerifyHashedPassword）
- 分頁：沒有使用套件，用 LINQ 的 Skip / Take 寫

## 功能

### 前台

- 首頁：遊艇圖片輪播、最新消息
- 遊艇：Interior 圖片輪播、Layout & deck plan、Specification
- 最新消息：列表（有分頁）和內頁
- 經銷商：依國家分類
- 聯絡我們：聯絡表單，送出前要同意隱私權條款

### 後台

- 登入：只有管理員可以進入，沒有開放註冊
- 遊艇：新增、修改、軟刪除、上架 / 下架、往上 / 往下排序
- 遊艇附件：封面圖、Gallery 和 Layout 圖片、PDF 下載檔
- 最新消息：新增、修改、軟刪除、上架 / 下架、封面圖、PDF 下載檔、分頁
- 經銷商：新增、修改、刪除、封面圖、國家管理
- 聯絡我們：聯絡頁設定、客戶留言（可以標記已處理）、隱私權條款版本管理

## 專案結構

以下資料夾都在 `TayanaYacht/` 底下：

- `Areas/Admin`：後台的 Controller 和 View
- `Controllers`：前台的 Controller
- `Models`：資料模型和 ViewModel
- `Views`：前台的 View
- `Content`：CSS 和圖片
- `Files`：上傳的檔案
- `Migrations`：Entity Framework 的 Migration

## 如何執行

1. clone 專案
   ```
   git clone https://github.com/unclehank5255/TayanaYacht.git
   ```
2. 用 Visual Studio 打開，還原 NuGet 套件
3. 修改 `Web.config` 裡的資料庫連線字串
4. 在套件管理器主控台執行 `Update-Database`
5. 按 F5 執行

網頁入口：

- 前台首頁：`/`
- 後台：`/Admin`

## 作者

unclehank5255 — [GitHub](https://github.com/unclehank5255)
