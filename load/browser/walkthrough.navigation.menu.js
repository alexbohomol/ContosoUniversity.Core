import { browser } from "k6/browser";
import { fail } from 'k6';
import { expect } from "https://jslib.k6.io/k6-testing/0.6.1/index.js";
import configs from '../configs.js';

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
      },
      exec: "uiWalkthrough"
    }
  }
};

export async function uiWalkthrough() {
  const page = await browser.newPage();

  try {

    await page.goto(configs.rootUrl);
    await expect.soft(page.locator("main > div.jumbotron > h1")).toHaveText("Contoso University");

    await page.goto(`${configs.rootUrl}/Home/About`);
    await expect.soft(page.locator("main > h2")).toHaveText("Student Body Statistics");

    await page.goto(`${configs.rootUrl}/Students`);
    await expect.soft(page.locator("main > h2")).toHaveText("Students");

    await page.goto(`${configs.rootUrl}/Courses`);
    await expect.soft(page.locator("main > h2")).toHaveText("Courses");

    await page.goto(`${configs.rootUrl}/Instructors`);
    await expect.soft(page.locator("main > h2")).toHaveText("Instructors");

    await page.goto(`${configs.rootUrl}/Departments`);
    await expect.soft(page.locator("main > h1")).toHaveText("Departments");

  } catch (error) {
    fail(`Browser iteration failed: ${error.message}`);
  } finally {
    await page.close();
  }
}
