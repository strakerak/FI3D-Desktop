from PIL import Image
import numpy as np

print("hi")

pixels = []
imageTxt = ""

#import image
f = open("C:/Users/Samy/Desktop/FI3D-Unity-Android/Assets/Materials/DemoData/Cardiac/Slices/Data/Phase_1.txt" , "r")
imageTxt = f.readline()

indexMax = 192*256
index = 0
imageArr = imageTxt.split(",")
for x in range(0,10):
    for i in range(0,256):
        row=[]
        for j in range(0,192):
            row.append(imageArr[index])
            index+=1
        pixels.append(row)

    array = np.array(pixels, dtype=np.uint8)

    new_image = Image.fromarray(array)
    new_image.save(f"Phase_1_{x}.png")
    pixels=[]
    

#print(len(pixels))
#print(pixels)


