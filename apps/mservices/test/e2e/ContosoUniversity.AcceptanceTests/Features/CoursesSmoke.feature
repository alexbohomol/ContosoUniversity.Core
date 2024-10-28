@Courses
Feature: Courses functionality
Smoke test

    Scenario: 01_ User is able to view the full list of courses
        Given user is on the Courses area landing page
        Then user is able to view the following list of courses
          | Course Code | Title          | Credits | Department  |
          | 4041        | Macroeconomics | 3       | Economics   |
          | 4022        | Microeconomics | 3       | Economics   |
          | 3141        | Trigonometry   | 4       | Mathematics |
          | 2021        | Composition    | 3       | English     |
          | 1045        | Calculus       | 4       | Mathematics |
          | 1050        | Chemistry      | 3       | Engineering |
          | 2042        | Literature     | 4       | English     |
        And the page title is "Courses - Contoso University"

    Scenario: 02_ User can navigate to "Create Course" page
        Given user is on the Courses area landing page
        When user clicks "Create New" link
        Then the "/Create" page opens successfully
        And the page title is "Create - Contoso University"

    Scenario: 03_ User can create new Course
        Given user is on the "/Create" page
        When user enters following details on form
          | Course Code | Title     | Credits | Department  |
          | 1010        | Computers | 5       | Engineering |
        And user submits the form
        Then user is redirected to the Courses area landing page
        And the page title is "Courses - Contoso University"
        And user is able to view the full list of courses
        And including the course just submitted

    Scenario: 04_ User can navigate to "Course Details" page
        Given user is on the Courses area landing page
        When user clicks "Details" link for course "1010"
        Then the "/Details" page with identifier opens successfully
        And the page title is "Details - Contoso University"
        And user is able to see the following details on course
          | Course Code | Title     | Credits | Department  |
          | 1010        | Computers | 5       | Engineering |

    Scenario: 05_ User can navigate to "Edit Course" page
        Given user is on the Courses area landing page
        When user clicks "Edit" link for course "1010"
        Then the "/Edit" page with identifier opens successfully
        And the page title is "Edit - Contoso University"
        And user is able to see the following course details to edit
          | Course Code | Title     | Credits | Department  |
          | 1010        | Computers | 5       | Engineering |

    Scenario: 06_ User can edit Course
        Given user is on the "Edit" page for course "1010"
        When user enters following details on form
          | Title            | Credits | Department  |
          | Computer Algebra | 3       | Mathematics |
        And user submits the form
        Then user is redirected to the Courses area landing page
        And the page title is "Courses - Contoso University"
        And user is able to view the full list of courses
        And including the course just submitted

    Scenario: 07_ User can navigate to "Delete Course" page
        Given user is on the Courses area landing page
        When user clicks "Delete" link for course "1010"
        Then the "/Delete" page with identifier opens successfully
        And the page title is "Delete - Contoso University"
        And user is able to see the following details on course
          | Course Code | Title            | Credits | Department  |
          | 1010        | Computer Algebra | 3       | Mathematics |

    Scenario: 08_ User can delete course
        Given user is on the "Delete" page for course "1010"
        When user submits the form
        Then user is redirected to the Courses area landing page
        And the page title is "Courses - Contoso University"
        And user is able to view the full list of courses
        And excluding the course just deleted
          | Course Code | Title            | Credits | Department  |
          | 1010        | Computer Algebra | 3       | Mathematics |