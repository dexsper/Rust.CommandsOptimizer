# Rust Commands Optimizer

This project provides a solution for optimizing memory usage on Rust game servers by leveraging Oxide, Harmony, and ZString. It focuses on enhancing performance and reducing memory allocations associated with `SendClientCommand`.

## Overview

Rust game servers can experience performance bottlenecks due to excessive memory allocations when handling commands. This solution aims to address these issues by:

- Replacing `StringBuilder` with `ZString` to minimize allocations.
- Overriding `ConsoleSystem.BuildCommand` for optimized command processing.
- Using `ReadOnlySpan` for direct writing of commands to `NetWrite` (Stream).

<table>
  <tr>
    <th>Before</th>
    <th>After</th>
  </tr>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/769d4dbb-09c7-4692-8a73-e0cf11741e37" alt="metrics before" /></td>
    <td><img src="https://github.com/user-attachments/assets/26014066-b458-4bcd-ae4f-aff3c5b62658" alt="metrics after" /></td>
  </tr>
   <tr>
      <th colspan="2">Benchmarks</th>
   </tr>
  <tr>
    <td colspan="2" align="center"><image src="https://github.com/user-attachments/assets/d5357301-8042-45f3-843d-7e391527d591" alt="benchmarks" /></td>
  </tr>
</table>

## Prerequisites

- **Oxide**: [Oxide Framework](https://umod.org/)
- **Harmony**: [Harmony GitHub Repository](https://github.com/pardeike/Harmony)
- **ZString**: [ZString GitHub Repository](https://github.com/Cysharp/ZString)

## Installation
1. **Build ZString as an Oxide Extension**:
   - Follow the instructions in the [ZString GitHub Repository](https://github.com/Cysharp/ZString) to build ZString as an Oxide Extension.

2. **Add Dependencies**:
   - Ensure Oxide, Harmony, and the built ZString extension are referenced in your project.

4. **Load plugin**:
   - Move [CommandsOptimizer.cs](https://github.com/dexsper/Rust.CommandsOptimizer/blob/main/CommandsOptimizer.cs) to your plugin folder

## Contributing

Feel free to submit issues or pull requests. Contributions are welcome to further improve this solution.
