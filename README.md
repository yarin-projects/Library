# Library Management System

## Overview

The Library Management System provides functionalities for users to interact with the library's collection, including browsing, borrowing, and purchasing books and journals. It supports both guest and registered user access with a variety of features for managing items and user accounts.

The project was completed on April, 2024.

## Features

### MainWindow

- **Guest Access**: Allows guests to browse the library collection without authentication.
- **Existing Account Login**: Users can log in to access personalized features.
- **New Account Creation**: New users can create accounts to gain access to additional features.
- **Navigation**: Intuitive transitions between login, account creation, and library browsing functionalities.

### Guest Access

1. **GuestView**: Interface for guests to explore the library's collection.
2. **GuestBrowseCollectionView**: Window for searching and browsing the library collection.
3. **GuestSelectedBookView**: Displays detailed information about a selected book.
4. **GuestSelectedJournalView**: Displays detailed information about a selected journal.

### User-Related Windows

1. **UserView**: Main interface for registered users.
2. **BrowseCollectionView**: Allows users to browse the library collection for borrowing or purchasing.
3. **UserSelectedBookView**: Detailed view of a selected book for review.
4. **UserSelectedJournalView**: Detailed view of a selected journal for review.

### User-Related Borrowed Windows

1. **BorrowedCollectionView**: Shows borrowed books/journals.
2. **UserSelectedBorrowedBookView**: Details of a borrowed book.
3. **UserSelectedBorrowedJournalView**: Details of a borrowed journal.

### User-Related Bought Windows

1. **BoughtCollectionView**: Displays purchased books/journals.
2. **UserSelectedBoughtBookView**: Details of a purchased book.
3. **UserSelectedBoughtJournalView**: Details of a purchased journal.

## Classes and Interfaces

### LibCollection Class

- **Purpose**: Manages a collection of items (books and journals).
- **Initialization**: Loads data from XML files and creates directories if needed.
- **Data Management**: Add, remove, and retrieve items; sort items; handle exceptions.
- **Singleton Pattern**: Ensures a single instance of the class.

### UserManager Class

- **Purpose**: Manages user-related operations.
- **Initialization**: Sets up storage paths and lists.
- **User Management**: Add, clear, and delete users.
- **Item Retrieval**: Methods to get items by title, type, price, and ISBN/ISSN.
- **Exception Handling**: Custom exceptions for various scenarios.

### AbstractItem Class

- **Purpose**: Blueprint for library items.
- **Attributes**: Title, author(s), publication year, identifier, price, type, category.
- **Methods**: Get and set item attributes.

### Book Class

- **Purpose**: Represents book items.
- **Additional Properties**: Edition, publisher, genre, ISBN.
- **Discount Management**: Methods for handling book price discounts.

### Journal Class

- **Purpose**: Represents journal items.
- **Additional Properties**: Volume, issue, publisher, ISSN, publication frequency.

### User Class

- **Purpose**: Represents registered users.
- **Attributes**: Username, password.

### Repository Class

- **Purpose**: Facilitates data persistence using XML serialization.
- **Methods**: SaveData and LoadData for saving and loading data.

### Enums

- **BookCategories**: Categories for books (e.g., Horror, Fantasy).
- **JournalCategories**: Categories for journals (e.g., Science, Biography).
- **Months**: Represents months of the year for date-related calculations.

### Exceptions

- **ItemNotFoundException**: Handles cases where an item is not found.
- **ManagerHasNoUserRegisteredException**: Thrown when no user is registered.
- **UserAlreadyInManagerException**: Thrown when attempting to add an existing user.

### ILibCollection Interface

- **Purpose**: Standardized interface for collections managing AbstractItem types.
- **Methods**: Add, Remove, GetItemsByTitle, SortByTitle, SortByType, etc.

### SortDirection Class

- **Purpose**: Defines sorting directions (ascending and descending).

## Testing

### RepositoryTests

- **Purpose**: Verifies functionality of the Repository class.
- **Tests**: Load and reload data, verify item counts.

### BookTests

- **Purpose**: Verifies functionality of the Book class.
- **Tests**: Construction, copying details, price recovery.

### JournalTests

- **Purpose**: Verifies functionality of the Journal class.
- **Tests**: Construction, copying details, price recovery.

### UserManagerTests

- **Purpose**: Verifies functionality of the UserManager class.
- **Tests**: User management, item retrieval, sorting.

### LibCollectionTests

- **Purpose**: Verifies functionality of the LibCollection class.
- **Tests**: Construction, data reloading, item retrieval, sorting.
