# Bank Statement PDF Parser (.NET)

A robust and intelligent system that extracts financial transactions from PDF bank statements and automatically classifies them into Debit and Credit using balance-based logic.

---

## Features

* Parse unstructured PDF bank statements
* Supports Indian number format (e.g., 1,00,000)
* Extracts:

  * Date
  * Description
  * Debit
  * Credit
  * Balance
  * Auto-detects Debit/Credit using balance difference (no keyword dependency)
  * Handles real-world PDF challenges:

  * Merged text (no spacing)
  * Special characters (–, etc.)
  * Noise and headers
  * Stores parsed transactions into database

---

## Tech Stack

* .NET (C#)
* PDF parsing using PdfPig
* Regex-based text processing
* Oracle Database

---

## Core Logic (Key Highlight)

Instead of relying on unreliable keywords, this system uses:

**Balance Difference Method:**

* If balance increases → Credit
* If balance decreases → Debit

This ensures high accuracy across different bank formats.

---

## Sample Input (PDF Extract)

01-Apr-2025Opening Balance1,00,000
01-Apr-2025Purchase - Raw Materials15,00085,000
02-Apr-2025Sale - Invoice #S20120,0001,05,000

---

## Sample Output

| Date        | Description              | Debit | Credit | Balance |
| ----------- | ------------------------ | ----- | ------ | ------- |
| 01-Apr-2025 | Purchase - Raw Materials | 15000 | 0      | 85000   |
| 02-Apr-2025 | Sale - Invoice #S201     | 0     | 20000  | 105000  |

---

## Challenges Solved

* Parsing messy PDF text with no clear structure
* Handling Indian currency format
* Avoiding incorrect number extraction (invoice numbers)
* Designing a reliable financial classification logic

---

## Future Enhancements

* Multi-bank format support
* UI for PDF upload & preview
* REST API integration
* Bulk processing support

---

## Author

Developed as a real-world financial data processing solution to automate bank statement parsing and transaction classification.

---
