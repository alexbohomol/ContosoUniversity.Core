@Departments
Feature: Departments functionality
Smoke test

    Scenario: 01_ User is able to view the full list of departments
        Given user is on the Departments area landing page
        Then user is able to view the following list of departments
          | Name        | Budget      | Start Date | Administrator    |
          | Economics   | ¤100,000.00 | 2007-09-01 | Kapoor, Candace  |
          | Engineering | ¤350,000.00 | 2007-09-01 | Harui, Roger     |
          | Mathematics | ¤100,000.00 | 2007-09-01 | Fakhouri, Fadi   |
          | English     | ¤350,000.00 | 2007-09-01 | Abercrombie, Kim |
        And the page title is "Departments - Contoso University"

    Scenario: 02_ User can navigate to "Create Department" page
        Given user is on the Departments area landing page
        When user clicks "Create New" link
        Then the "/Create" page opens successfully
        And the page title is "Create - Contoso University"

    Scenario: 03_ User can create new Department
        Given user is on the "/Create" page
        When user enters following details on form
          | Name        | Budget        | Start Date | Administrator |
          | Informatics | ¤1,000,000.00 | 2021-09-01 | Zheng, Roger  |
        And user submits the form
        Then user is redirected to the Departments area landing page
        And the page title is "Departments - Contoso University"
        And user is able to view the full list of departments
        And including the department just submitted

    Scenario: 04_ User can navigate to "Department Details" page
        Given user is on the Departments area landing page
        When user clicks "Details" link for department "Informatics"
        Then the "/Details" page with identifier opens successfully
        And the page title is "Details - Contoso University"
        And user is able to see the following details on department
          | Name        | Budget        | Start Date | Administrator |
          | Informatics | ¤1,000,000.00 | 2021-09-01 | Zheng, Roger  |

    Scenario: 05_ User can navigate to "Edit Department" page
        Given user is on the Departments area landing page
        When user clicks "Edit" link for department "Informatics"
        Then the "/Edit" page with identifier opens successfully
        And the page title is "Edit - Contoso University"
        And user is able to see the following department details to edit
          | Name        | Budget        | Start Date | Administrator |
          | Informatics | ¤1,000,000.00 | 2021-09-01 | Zheng, Roger  |

    Scenario: 06_ User can edit Department
        Given user is on the "Edit" page for department "Informatics"
        When user enters following details on form
          | Name      | Budget        | Start Date | Administrator |
          | Computers | ¤1,100,000.00 | 2022-09-01 | Harui, Roger  |
        And user submits the form
        Then user is redirected to the Departments area landing page
        And the page title is "Departments - Contoso University"
        And user is able to view the full list of departments
        And including the department just submitted

    Scenario: 07_ User can navigate to "Delete Department" page
        Given user is on the Departments area landing page
        When user clicks "Delete" link for department "Computers"
        Then the "/Delete" page with identifier opens successfully
        And the page title is "Delete - Contoso University"
        And user is able to see the following details on department
          | Name      | Budget        | Start Date | Administrator |
          | Computers | ¤1,100,000.00 | 2022-09-01 | Harui, Roger  |

    Scenario: 08_ User can delete department
        Given user is on the "Delete" page for department "Computers"
        When user submits the form
        Then user is redirected to the Departments area landing page
        And the page title is "Departments - Contoso University"
        And user is able to view the full list of departments
        And excluding the department just deleted
          | Name      | Budget        | Start Date | Administrator |
          | Computers | ¤1,100,000.00 | 2022-09-01 | Harui, Roger  |