@Students
Feature: Students functionality
Smoke test

    Scenario: 01_ User is able to view the full list of students
        Given user is on the Students area landing page
        Then user is able to view the following list of students
          | Last Name | First Name | Enrollment Date |
          | Alexander | Carson     | 2010-09-01      |
          | Alonso    | Meredith   | 2012-09-01      |
          | Anand     | Arturo     | 2013-09-01      |
        And the page title is "Index - Contoso University"

    Scenario: 02_ User can navigate to "Create Student" page
        Given user is on the Students area landing page
        When user clicks "Create New" link
        Then the "/Create" page opens successfully
        And the page title is "Create - Contoso University"

    Scenario: 03_ User can create new Student
        Given user is on the "/Create" page
        When user enters following details on form
          | Last Name | First Name | Enrollment Date |
          | Alexander | Alpert     | 2022-01-17      |
        And user submits the form
        Then user is redirected to the Students area landing page
        And the page title is "Index - Contoso University"
        And user is able to view the full list of students
        And including the student just submitted

    Scenario: 04_ User can navigate to "Student Details" page
        Given user is on the Students area landing page
        When user clicks "Details" link for student "Alpert"
        Then the "/Details" page with identifier opens successfully
        And the page title is "Details - Contoso University"
        And user is able to see the following details on student
          | Last Name | First Name | Enrollment Date |
          | Alexander | Alpert     | 2022-01-17      |

    Scenario: 05_ User can navigate to "Edit Student" page
        Given user is on the Students area landing page
        When user clicks "Edit" link for student "Alpert"
        Then the "/Edit" page with identifier opens successfully
        And the page title is "Edit - Contoso University"
        And user is able to see the following student details to edit
          | Last Name | First Name | Enrollment Date |
          | Alexander | Alpert     | 2022-01-17      |

    Scenario: 06_ User can edit Student
        Given user is on the "Edit" page for student "Alpert"
        When user enters following details on form
          | Last Name | First Name | Enrollment Date |
          | Alex      | Cupertino  | 2022-01-31      |
        And user submits the form
        Then user is redirected to the Students area landing page
        And the page title is "Index - Contoso University"
        And user is able to view the full list of students
        And including the student just submitted

    Scenario: 07_ User can navigate to "Delete Student" page
        Given user is on the Students area landing page
        When user clicks "Delete" link for student "Cupertino"
        Then the "/Delete" page with identifier opens successfully
        And the page title is "Delete - Contoso University"
        And user is able to see the following details on student
          | Last Name | First Name | Enrollment Date |
          | Alex      | Cupertino  | 2022-01-31      |

    Scenario: 08_ User can delete student
        Given user is on the "Delete" page for student "Cupertino"
        When user submits the form
        Then user is redirected to the Students area landing page
        And the page title is "Index - Contoso University"
        And user is able to view the full list of students
        And excluding the student just deleted
          | Last Name | First Name | Enrollment Date |
          | Alex      | Cupertino  | 2022-01-31      |