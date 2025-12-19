import { browser } from "k6/browser";
import { fail } from "k6";
import { expect } from "https://jslib.k6.io/k6-testing/0.6.1/index.js";
import configs from "../configs.js";

export const options = {
  scenarios: {
    ui: {
      executor: "shared-iterations",
      vus: 1,
      iterations: 50,
      options: {
        browser: {
          type: "chromium",
        },
      },
    },
  },
};

async function clickNavigationItem(page, linkText) {
  const link = page.getByRole("link", { name: linkText, exact: true });
  await expect(link).toBeVisible();
  await Promise.all([
    page.waitForNavigation({ waitUntil: "load" }),
    link.click(),
  ]);
}

async function uiWalkthrough() {
  const page = await browser.newPage();

  try {

    await page.goto(configs.rootUrl);
    await expect.soft(page.locator("main > div.jumbotron > h1")).toHaveText("Contoso University");

    await clickNavigationItem(page, "About");
    await expect.soft(page.locator("main > h2")).toHaveText("Student Body Statistics");

    await clickNavigationItem(page, "Students");
    await expect.soft(page.locator("main > h2")).toHaveText("Students");

    await clickNavigationItem(page, "Courses");
    await expect.soft(page.locator("main > h2")).toHaveText("Courses");

    await clickNavigationItem(page, "Instructors");
    await expect.soft(page.locator("main > h2")).toHaveText("Instructors");

    await clickNavigationItem(page, "Departments");
    await expect.soft(page.locator("main > h1")).toHaveText("Departments");

    await clickNavigationItem(page, "Home");
    await expect.soft(page.locator("main > div.jumbotron > h1")).toHaveText("Contoso University");
  } catch (error) {
    fail(`Browser iteration failed: ${error.message}`);
  } finally {
    await page.close();
  }
}

export { uiWalkthrough };
export default uiWalkthrough;
