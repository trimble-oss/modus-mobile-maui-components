# Security Policy

## Supported Versions

We release patches for security vulnerabilities in the following versions:

| Version | Supported          |
| ------- | ------------------ |
| Latest  | :white_check_mark: |
| < Latest| :x:                |

## Reporting a Vulnerability

**Please do not report security vulnerabilities through public GitHub issues.**

Instead, please report security vulnerabilities using the official Trimble cybersecurity reporting form:

- **Report Form**: [Trimble Cybersecurity Issue Reporting Form](https://www.trimble.com/en/our-commitment/responsible-business/data-privacy-and-security/report-cybersecurity-issues/form)

You may also contact the repository maintainers directly:

- **Email**: [trimble-oss-contrib-admins-ug@trimble.com](mailto:trimble-oss-contrib-admins-ug@trimble.com)
- **Subject Line**: [SECURITY] Trimble Modus Mobile MAUI Components - [Brief description]

Please include the following information in your report:

- Type of issue (e.g. buffer overflow, SQL injection, cross-site scripting, etc.)
- Full paths of source file(s) related to the manifestation of the issue
- The location of the affected source code (tag/branch/commit or direct URL)
- Any special configuration required to reproduce the issue
- Step-by-step instructions to reproduce the issue
- Proof-of-concept or exploit code (if possible)
- Impact of the issue, including how an attacker might exploit the issue

## Response Process

1. **Acknowledgment**: We will acknowledge receipt of your vulnerability report within 48 hours
2. **Investigation**: We will investigate and validate the vulnerability
3. **Resolution**: We will work on a fix and coordinate disclosure timing with you
4. **Disclosure**: We will publicly disclose the vulnerability after a fix is available

## Security Best Practices

When using Trimble Modus Mobile MAUI Components:

- Always use the latest version of the library
- Keep your .NET MAUI framework updated
- Follow secure coding practices in your applications
- Regularly update your dependencies
- Monitor security advisories for related packages

## Security Features

This library includes the following security measures:

- Regular dependency updates through Dependabot
- Automated security scanning via CodeQL
- Code review requirements for all changes
- Vulnerability assessments on releases

## Contact

For questions about this security policy, please contact [modus-mobile-contributors-ug@trimble.com](mailto:modus-mobile-contributors-ug@trimble.com).