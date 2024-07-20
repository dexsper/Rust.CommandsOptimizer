# Rust Commands Optimizer

This project provides a solution for optimizing memory usage on Rust game servers by leveraging Oxide, Harmony, and ZString. It focuses on enhancing performance and reducing memory allocations associated with `SendClientCommand`.

## Overview

Rust game servers can experience performance bottlenecks due to excessive memory allocations when handling commands. This solution aims to address these issues by:

- Replacing `StringBuilder` with `ZString` to minimize allocations.
- Overriding `ConsoleSystem.BuildCommand` for optimized command processing.
- Using `ReadOnlySpan` for direct writing of commands to `NetWrite` (Stream).

## Features

- **Memory Optimization**: Significantly reduces memory allocations by avoiding unnecessary intermediate objects.
- **Efficient Command Processing**: Utilizes `ReadOnlySpan` for direct command writing, improving performance.
- **Enhanced Performance**: Lower memory usage and improved server responsiveness.

![image](https://github.com/user-attachments/assets/d5357301-8042-45f3-843d-7e391527d591)

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
