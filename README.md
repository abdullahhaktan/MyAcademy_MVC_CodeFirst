# 🛡️ InsureVision AI - AI & Machine Learning Powered Insurance System

**InsureVision AI**, sigortacılık süreçlerini yapay zeka ve makine öğrenmesi ile modernize eden kurumsal bir web platformudur. M&Y Yazılım Eğitim Akademi bünyesinde, **Murat Yücedağ** ve **Erhan Gündüz** hocalarımın rehberliğinde geliştirdiğim 9. projemdir.

---

## ⚙️ Teknik Detaylar & Analitik Özellikler

### 📈 ML.NET ile Satış Projeksiyonu
Microsoft'un **ML.NET** kütüphanesi kullanılarak **SSA (Singular Spectrum Analysis)** algoritması ile geçmiş poliçe verileri analiz edilmiştir. 
* **Gelecek Tahmini:** Gelecek 3 ayın satış yoğunluğu %95 güven aralığı ile tahmin edilerek yönetimsel bir karar destek mekanizması oluşturulmuştur.

### 🤖 Gelişmiş NLP & Dil Teknolojileri
Projede operasyonel verimliliği artırmak adına üç farklı yapay zeka katmanı entegre edilmiştir:

* **Hugging Face (BART-Large-MNLI):** Gelen müşteri mesajları (Şikayet, Talep, Destek vb.) otomatik olarak kategorize edilir.
* **OpenAI GPT:** Mesaj dili otomatik tespit edilerek, müşteriye kendi dilinde kurumsal ve profesyonel otomatik yanıtlar döner.
* **Google Gemini AI:** Poliçe türü ve tutarına göre profesyonel poliçe açıklama metinlerini dinamik olarak üretir.

### 🛡️ Çift Katmanlı Log Yönetimi
Sistemin izlenebilirliği ve güvenliği için kapsamlı bir **Audit Log** mimarisi kurulmuştur:
* **AdminLogService:** Yönetici hareketlerini takip eder.
* **CustomerLogService:** Müşteri hareketlerini IP adresi ve işlem detayı bazlı kaydeder.

### 📅 Dinamik Dashboard & Analiz
**Chart.js** ve **AJAX** entegrasyonu sayesinde:
* Son 6 ayın trend analizi.
* Aktif poliçe oranları ve bekleyen talepler anlık olarak görselleştirilmektedir.

---

## 🚀 Mimari ve Teknolojiler

* **Core:** ASP.NET MVC, Entity Framework (Code First)
* **Intelligence:** ML.NET, OpenAI API, Gemini API, Hugging Face Inference API
* **Security:** Role-Based Authorization, Advanced Logging System (Audit Log)
* **Design:** Responsive Dashboard, Chart.js, SweetAlert, DTO Pattern

---

## 📊 Veri Odaklı Yaklaşım
Bu çalışma; sadece veriyi depolayan değil, **NLP** ile metni anlayan, **ML.NET** ile geleceği tahmin eden ve her adımda sistemsel güvenliği (Logging) ön planda tutan bir altyapı sunmaktadır.

---

## 🔗 Bağlantılar
* **GitHub Repo:** [https://lnkd.in/dkSAXPhV](https://lnkd.in/dkSAXPhV)

---
`#dotnet` `#csharp` `#mlnet` `#machinelearning` `#openai` `#geminiapi` `#huggingface` `#insurtech` `#aspnetmvc` `#softwareengineering` `#artificialintelligence` `#nlp` `#webdevelopment`

## Ekran Fotoğrafları

<img width="271" height="439" alt="Ekran görüntüsü 2026-01-18 112222" src="https://github.com/user-attachments/assets/10c85985-d200-496f-be98-89cf05a00c37" />

---

<img width="295" height="255" alt="Ekran görüntüsü 2026-01-18 112253" src="https://github.com/user-attachments/assets/c8143bce-782a-4f81-9648-07e35f5fee69" />

---

<img width="308" height="385" alt="Ekran görüntüsü 2026-01-18 112322" src="https://github.com/user-attachments/assets/f6631984-eb3a-40af-b76a-82de1c435605" />

---

<img width="286" height="391" alt="Ekran görüntüsü 2026-01-18 112343" src="https://github.com/user-attachments/assets/8cccb23c-7015-4b99-a3be-c879198c6fc6" />

---

<img width="959" height="437" alt="Ekran görüntüsü 2026-01-18 112835" src="https://github.com/user-attachments/assets/c8949e5b-3a20-4369-bdb9-457c6ec8c332" />

---

<img width="959" height="436" alt="Ekran görüntüsü 2026-01-18 112858" src="https://github.com/user-attachments/assets/0afe2ad7-60ce-4976-a6c3-e95f364da0fe" />

<img width="959" height="435" alt="Ekran görüntüsü 2026-01-18 113102" src="https://github.com/user-attachments/assets/297e08f7-f07f-4dca-b43e-84ca179d72b6" />

---

<img width="958" height="437" alt="Ekran görüntüsü 2026-01-18 113134" src="https://github.com/user-attachments/assets/32b7091b-e7a7-406a-808e-8c7e941ca3d2" />
