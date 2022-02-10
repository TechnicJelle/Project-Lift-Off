#include <Mouse.h>

const int activate = 2;
const int leftClick = 8;
const int rightClick = 7;

const int axisX = A1;
const int axisY = A0;

const int led = 12;

int range = 12;
int resDelay = 5;
int threshold = range / 4;
int center = range / 2;

int screenWidth = 1920;
int screenHeight = 1080;
int screenCenter[] = {screenWidth / 2, screenHeight / 2};

bool isActive = false;
int lastActiveState = LOW;

void setup() {
    pinMode(activate, INPUT);
    pinMode(leftClick, INPUT);
    pinMode(rightClick, INPUT);
    pinMode(axisX, INPUT);
    pinMode(axisY, INPUT);
    
    pinMode(led, OUTPUT);

    Mouse.begin();
}

void loop() {
    int activeState = digitalRead(activate);

    if (activeState != lastActiveState) {
        if (activeState == HIGH) {
            isActive = !isActive;

            digitalWrite(led, isActive);
        }
    }

    lastActiveState = activeState;

    int xRead = readAxis(A1);
    int yRead = readAxis(A0);

    if (isActive) {
        Mouse.move(xRead, yRead, 0);
    }

    if (digitalRead(leftClick) == 1) {
        if (!Mouse.isPressed(MOUSE_LEFT)) {
            Serial.println("click");
            Mouse.press(MOUSE_LEFT);
        }
    } else if (digitalRead(rightClick) == 1) {
        if (!Mouse.isPressed(MOUSE_RIGHT)) {
            Mouse.press(MOUSE_RIGHT);
        }
    } else {
        if(Mouse.isPressed(MOUSE_LEFT)) {
            Mouse.release(MOUSE_LEFT);
        } else if (Mouse.isPressed(MOUSE_RIGHT)) {
            Mouse.release(MOUSE_RIGHT);
        }
    }
    

    delay(resDelay);    
}

int readAxis(int axis) {
    int reading = analogRead(axis);

    reading = map(reading, 0, 1023, 0, range);

    int distance = reading - center;

    if (abs(distance) < threshold) {
        distance = 0;
    }

    return distance;
}
