# Technical details

The project is written in .NET Framework, utilizing the WinForms libraries.

# App description

## Factory Overview:  
- The factory is a central entity that manages the production of details.  
- It consists of a series of interconnected workshops, each responsible for a specific stage of the detail production process.
## Operations
- There are several types of operations, each of which can be performed in specific workshops.
- Each operation has unmutable time.
## Workshops:  
- There are five distinct workshops, each representing a set of available operations for the details.  
- Workshop 1: turning, milling, bending  
- Workshop 2: turning, drilling, welding  
- Workshop 3: grinding, flushing  
- Workshop 4: primer, painting, drying  
- Workshop 5: painting, drying  
## Detail Processing Flow:
- Details move from one workshop to another in a predefined sequence.
- The available workshop takes the first item from the list, processes it, and returns it.
- When a workshop capable of performing the next operation becomes available, it will take the processed item.
## Order Management:  
- The project includes a system for managing the order in which details are processed through the workshops.
## Simulation Controls:
- The C# project includes simulation controls to start, pause, and stop the production simulation.
- Users can observe the progress of details through the workshops and analyze the efficiency of the production process.
## Reporting and Analytics:
- Users can access real-time and historical information, including processing times at each workshop, total production time for individual details, and overall production cycle time.

![1](https://github.com/edomozh/factory-simulation/assets/15713802/f993e963-0c69-43d3-a8f8-ee0184cd9c74)
![2](https://github.com/edomozh/factory-simulation/assets/15713802/f97384ea-7f27-4317-b5ba-a0b875a15df5)
