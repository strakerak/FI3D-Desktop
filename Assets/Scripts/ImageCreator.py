from PIL import Image
import PIL
import numpy as np

print("hi")

pixels = []
imageTxt = ""

#import image
f = open("C:/Users/smmt/OneDrive/Desktop/Unity Projects/FI-Unity-Android-main/Assets/Materials/DemoData/Cardiac/Slices/Data/Phase_0.txt" , "r")
imageTxt = f.readline()
print(len(imageTxt))
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
    new_image = new_image.rotate(90, PIL.Image.NEAREST, expand=1)
    new_image.save(f"Phase_0_{x}.png")
    print("saved as",x)
    pixels=[]
    

#print(len(pixels))
#print(pixels)


