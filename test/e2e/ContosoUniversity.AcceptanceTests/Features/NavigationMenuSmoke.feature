@Navigation
Feature: User can navigate header menu
Smoke test

    Scenario: User can navigate to the site root
        Given user is on the site landing page
        When user clicks brand link in the navigation bar
        Then the "/" area opens successfully
        And the page title is "Home Page - Contoso University"

    Scenario: User can navigate to the Home area
        Given user is on the site landing page
        When user clicks "Home" link in the navigation bar
        Then the "/" area opens successfully
        And the page title is "Home Page - Contoso University"

    Scenario: User can navigate to the About area
        Given user is on the site landing page
        When user clicks "About" link in the navigation bar
        Then the "/Home/About" area opens successfully
        And the page title is "Student Body Statistics - Contoso University"

    Scenario: User can navigate to the Students area
        Given user is on the site landing page
        When user clicks "Students" link in the navigation bar
        Then the "/Students" area opens successfully
        And the page title is "Index - Contoso University"

    Scenario: User can navigate to the Courses area
        Given user is on the site landing page
        When user clicks "Courses" link in the navigation bar
        Then the "/Courses" area opens successfully
        And the page title is "Courses - Contoso University"

    Scenario: User can navigate to the Instructors area
        Given user is on the site landing page
        When user clicks "Instructors" link in the navigation bar
        Then the "/Instructors" area opens successfully
        And the page title is "Instructors - Contoso University"

    Scenario: User can navigate to the Departments area
        Given user is on the site landing page
        When user clicks "Departments" link in the navigation bar
        Then the "/Departments" area opens successfully
        And the page title is "Departments - Contoso University"