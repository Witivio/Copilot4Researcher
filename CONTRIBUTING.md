# Contributing to Copilot for Researcher

Thank you for your interest in contributing to **Copilot for Researcher**! We welcome all contributions—whether you're fixing bugs, improving documentation, adding features, or helping expand the data sources available. Follow the guidelines below to make the contribution process smooth and efficient.

## How to Contribute

### 1. Reporting Issues

If you've found a bug or have a suggestion for a new feature, please report it by opening a [GitHub issue](https://github.com/witivio/copilot4researcher/issues).

When reporting an issue, make sure to include:
- A clear description of the problem or feature request.
- Steps to reproduce the issue, if applicable.
- Relevant error messages, screenshots, or logs.
- Information about your environment (operating system, Azure setup, etc.).

### 2. Fork the Repository

To contribute code, you'll first need to fork the repository:
- Navigate to the main [Copilot for Researcher repository](https://github.com/witivio/copilot4researcher).
- Click the "Fork" button in the top right corner to create your copy of the repository.
- Clone your fork locally:
  ```bash
  git clone https://github.com/your-username/copilot-for-researcher.git
  cd copilot-for-researcher
  ```

### 3. Creating a Branch

Create a new branch for your changes. This will help to keep your work isolated from the main codebase.

```bash
git checkout -b feature-name
```

Use a descriptive name for your branch that reflects the changes you're making, such as `add-new-datasource` or `fix-auth-bug`.

### 4. Making Changes

Make your changes in the newly created branch. Be sure to follow these best practices:
- Write clear and concise code.
- Follow the existing coding style and conventions.
- Add or update tests for new features and bug fixes.
- Ensure that the code works with the existing Azure setup.

### 5. Testing

Before submitting your changes, make sure all existing tests pass, and add new tests if necessary:
- Run the test suite locally to ensure your changes don’t break anything.
- You can also add new test cases if you’re implementing a new feature or fixing a bug.

### 6. Creating a Pull Request

Once your changes are ready:
- Commit your changes:
  ```bash
  git commit -m "Description of the changes made"
  ```
- Push the branch to your fork:
  ```bash
  git push origin feature-name
  ```
- Open a Pull Request (PR) on the original repository by clicking the "New Pull Request" button.
  - Be sure to fill in the PR template with details about the changes you made.
  - Link any related issues in the PR description.

We will review your PR as soon as possible and may provide feedback or request changes.

### 7. Code Review

Your PR will undergo a code review process where maintainers and other contributors may:
- Provide feedback on your changes.
- Ask for additional improvements or modifications.
- Merge your changes once they meet the project’s requirements.

## Adding New Data Sources

One of the key features of **Copilot for Researcher** is its flexibility in adding new data sources. If you'd like to contribute by adding support for a new data source:
- Make sure the data source is relevant to researchers, especially in the life sciences domain.
- Follow the existing patterns for integrating external APIs or data repositories.
- Update documentation to describe the new data source and how it can be used.


## Getting Help

If you’re having trouble contributing or need help, feel free to reach out by:
- Opening a [discussion](https://github.com/witivio/copilot4researcher/discussions).
- Asking questions in our [GitHub Issues](https://github.com/witivio/copilot4researcher/issues).

We’re here to help!

---

Thank you for your contributions! We appreciate your effort to make **Copilot for Researcher** better for the entire scientific community.