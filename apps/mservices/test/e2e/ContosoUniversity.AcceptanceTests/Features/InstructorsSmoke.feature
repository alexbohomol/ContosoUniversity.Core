@Instructors
Feature: Instructors functionality
Smoke test

    Scenario: 01_ User is able to view the full list of instructors
        Given user is on the Instructors area landing page
        Then user is able to view the following list of instructors
          | Last Name   | First Name | Hire Date  | Office       |
          | Abercrombie | Kim        | 1995-03-11 |              |
          | Fakhouri    | Fadi       | 2002-07-06 | Smith 17     |
          | Harui       | Roger      | 1998-07-01 | Gowan 27     |
          | Kapoor      | Candace    | 2001-01-15 | Thompson 304 |
          | Zheng       | Roger      | 2004-02-12 |              |
        And the page title is "Instructors - Contoso University"

    Scenario: 02_ User can navigate to "Create Instructor" page
        Given user is on the Instructors area landing page
        When user clicks "Create New" link
        Then the "/Create" page opens successfully
        And the page title is "Create - Contoso University"

    Scenario: 03_ User can create new Instructor
        Given user is on the "/Create" page
        When user enters following details on form
          | Last Name | First Name | Hire Date  | Office |
          | Bohomol   | Alex       | 2021-12-31 |        |
        And user submits the form
        Then user is redirected to the Instructors area landing page
        And the page title is "Instructors - Contoso University"
        And user is able to view the full list of instructors
        And including the instructor just submitted

    Scenario: 04_ User can navigate to "Instructor Details" page
        Given user is on the Instructors area landing page
        When user clicks "Details" link for instructor "Bohomol"
        Then the "/Details" page with identifier opens successfully
        And the page title is "Details - Contoso University"
        And user is able to see the following details on instructor
          | Last Name | First Name | Hire Date  |
          | Bohomol   | Alex       | 12/31/2021 |

    Scenario: 05_ User can navigate to "Edit Instructor" page
        Given user is on the Instructors area landing page
        When user clicks "Edit" link for instructor "Bohomol"
        Then the "/Edit" page with identifier opens successfully
        And the page title is "Edit - Contoso University"
        And user is able to see the following instructor details to edit
          | Last Name | First Name | Hire Date  |
          | Bohomol   | Alex       | 2021-12-31 |

    Scenario: 06_ User can edit Instructor
        Given user is on the "Edit" page for instructor "Bohomol"
        When user enters following details on form
          | Last Name | First Name | Hire Date  | Office |
          | Bogomol   | Oleksandr  | 2022-01-31 |        |
        And user submits the form
        Then user is redirected to the Instructors area landing page
        And the page title is "Instructors - Contoso University"
        And user is able to view the full list of instructors
        And including the instructor just submitted

    Scenario: 07_ User can navigate to "Delete Instructor" page
        Given user is on the Instructors area landing page
        When user clicks "Delete" link for instructor "Bogomol"
        Then the "/Delete" page with identifier opens successfully
        And the page title is "Delete - Contoso University"
        And user is able to see the following details on instructor
          | Last Name | First Name | Hire Date |
          | Bogomol   | Oleksandr  | 1/31/2022 |

    Scenario: 08_ User can delete instructor
        Given user is on the "Delete" page for instructor "Bogomol"
        When user submits the form
        Then user is redirected to the Instructors area landing page
        And the page title is "Instructors - Contoso University"
        And user is able to view the full list of instructors
        And excluding the instructor just deleted
          | Last Name | First Name | Hire Date  |
          | Bogomol   | Oleksandr  | 2022-01-31 |