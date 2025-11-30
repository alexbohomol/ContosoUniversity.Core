import { browser } from "k6/browser";
import { fail } from 'k6';
import { expect } from "https://jslib.k6.io/k6-testing/0.5.0/index.js";

const BASE_URL = __ENV.BASE_URL || "http://contoso-mnlth-alb-963508264.eu-central-1.elb.amazonaws.com";

export const options = {
  scenarios: {
    ui: {
      executor: "shared-iterations",
      vus: 1,
      iterations: 50,
      options: {
        browser: {
          type: "chromium",
        }
      }
    }
  }
};

export default async function() {
  const page = await browser.newPage();

  try {

    await page.goto(BASE_URL);
    await expect.soft(page.locator("main > div.jumbotron > h1")).toHaveText("Contoso University");

    await page.goto(`${BASE_URL}/Home/About`);
    await expect.soft(page.locator("main > h2")).toHaveText("Student Body Statistics");

    await page.goto(`${BASE_URL}/Students`);
    await expect.soft(page.locator("main > h2")).toHaveText("Students");

    await page.goto(`${BASE_URL}/Courses`);
    await expect.soft(page.locator("main > h2")).toHaveText("Courses");

    await page.goto(`${BASE_URL}/Instructors`);
    await expect.soft(page.locator("main > h2")).toHaveText("Instructors");

    await page.goto(`${BASE_URL}/Departments`);
    await expect.soft(page.locator("main > h1")).toHaveText("Departments");

  } catch (error) {
    fail(`Browser iteration failed: ${error.message}`);
  } finally {
    await page.close();
  }
}
