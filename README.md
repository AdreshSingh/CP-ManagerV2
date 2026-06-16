# CP ManagerV2

A small .NET desktop / CLI utility that reads the clipboard, parses unstructured multiple-choice questions (MCQs), and converts them into a structured format (question, four options, explanation).

## Why this exists

Manual extraction of MCQs is slow and repetitive. A typical manual flow might be:

- copy → paste × 6 per MCQ × 40 MCQs = 240 operations

This tool reduces that to one paste per MCQ (the raw text) and an automated parse, turning 240 operations into 40 — roughly an 80%+ reduction in manual steps and a major productivity boost.

## Features

- Reads raw text from the system clipboard
- Automatically detects and segments: question, 4 options (A–D), and an explanation
- Outputs a clean, structured representation suitable for importing, copying, or further processing
- Works as a desktop app and can be used from the CLI

## Requirements

- .NET Framework 4.8
- Microsoft Visual Studio (recommended) or MSBuild to build from source

## Build & run

1. Open the solution in Visual Studio and build the project targeting .NET Framework 4.8.
2. Run the project (desktop) or launch the produced executable from the command line.

## Typical workflow

1. Copy the raw MCQ text into your clipboard (from a PDF, webpage, or other source).
2. Launch the app or run the CLI version.
3. The tool reads the clipboard, parses the MCQ into:
   - Question
   - Option A
   - Option B
   - Option C
   - Option D
   - Explanation
4. The structured output is written back to the clipboard and printed to the console (or displayed in the UI), ready to paste into your target system.

## Example

Raw clipboard input:

"Who wrote 'Pride and Prejudice'?\nA) Charlotte Brontë\nB) Jane Austen\nC) Emily Brontë\nD) Mary Shelley\nExplanation: A classic novel by Jane Austen."

Structured output:

Question: Who wrote 'Pride and Prejudice'?
A: Charlotte Brontë
B: Jane Austen
C: Emily Brontë
D: Mary Shelley
Explanation: A classic novel by Jane Austen.

## Notes

- The parser uses heuristics to handle common MCQ formats. If you encounter inputs that are not parsed correctly, please open an issue with an example so parsing rules can be improved.

## Contributing

Contributions and bug reports are welcome. Please submit issues or pull requests on the repository.

## License

This project does not include a license file in the repository by default. Add a LICENSE file if you want to specify one.

