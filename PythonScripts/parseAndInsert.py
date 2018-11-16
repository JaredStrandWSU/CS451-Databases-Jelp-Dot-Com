import json
import psycopg2
import os


def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")


def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'


def insertBussinessHoursCategories():
    print("Inserting businesses, hours, and categories")
    # reading the JSON file
    with open(os.path.dirname(os.path.realpath(__file__)) + '\\yelpData\\yelp_business.JSON', 'r') as f:
        line = f.readline()
        count_line = 0
        try:
            conn = psycopg2.connect("dbname='yelp' user='postgres' host='localhost' password='admin'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()
        categories = []

        while line:
            data = json.loads(line)
            business_sql_str = "INSERT INTO Businesses(business_id, name, address, state, " \
                      "city, postal_code, latitude, longitude, review_rating, review_count, " \
                      "open_status, num_checkIns) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + \
                      cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["address"]) \
                      + "','" + cleanStr4SQL(data["state"]) + "','" + \
                      cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["postal_code"]) \
                      + "'," + str(data["latitude"]) + "," + str(data["longitude"]) + "," + \
                      str(data["stars"]) + "," + str(data["review_count"]) + "," + \
                      int2BoolStr(data["is_open"]) + ",0);"
            try:
                cur.execute(business_sql_str)
            except:
                print("Insert to businessTABLE failed!")
                print(business_sql_str)
            conn.commit()

            hours = data["hours"]
            for day in hours:
                hours_sql_str = "INSERT INTO Hours(business_id, day, open, close) VALUES ('" + \
                                cleanStr4SQL(data['business_id']) + "','" + day + "','" + \
                                hours[day].split("-")[0] + "','" + hours[day].split("-")[1] + "');"
                try:
                    cur.execute(hours_sql_str)
                except:
                    print("Insert to hours failed!")
                    print(hours_sql_str)
                conn.commit()

            cats = data["categories"]
            for cat in cats:
                if cat not in categories:  # Only add categories to Categories table if haven't already
                    categories.append(cat)
                    categories_sql_str = "INSERT INTO Categories(name) VALUES ('" \
                        + cleanStr4SQL(cat) + "');"
                    try:
                        cur.execute(categories_sql_str)
                    except:
                        print("Insert to categories failed!")
                        print(categories_sql_str)
                    conn.commit()
                categories_sql_str = "INSERT INTO businessCategories(business_id, category) VALUES ('" \
                                     + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(cat) + "');"
                try:
                    cur.execute(categories_sql_str)
                except:
                    print("Insert to categories failed!")
                    print(categories_sql_str)
                conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    f.close()


def insertUsers():
    print("inserting users")
    # reading the JSON file
    with open(os.path.dirname(os.path.realpath(__file__)) + '\\yelpData\\yelp_user.JSON', 'r') as f:
        line = f.readline()
        count_line = 0

        try:
            conn = psycopg2.connect("dbname='yelp' user='postgres' host='localhost' password='admin'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            user_sql_str = "INSERT INTO Users(user_id, name, yelping_since, average_stars, " \
                                 "review_count, cool, funny, useful) " \
                      "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + \
                      cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["yelping_since"]) \
                      + "'," + str(data["average_stars"]) + "," + \
                      str(data["review_count"]) + "," + str(data["cool"]) \
                      + "," + str(data["funny"]) + "," + str(data["useful"]) + ");"
            try:
                cur.execute(user_sql_str)
            except:
                print("Insert to user table failed!")
                print(user_sql_str)
            conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    f.close()


def insertFriends():
    print("inserting friends")
    #reading the JSON file
    with open(os.path.dirname(os.path.realpath(__file__)) + '\\yelpData\\yelp_user.JSON', 'r') as f:
        line = f.readline()
        count_line = 0

        try:
            conn = psycopg2.connect("dbname='yelp' user='postgres' host='localhost' password='admin'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            friends = data["friends"]
            for friend in friends:
                friends_sql_str = "INSERT INTO Friends(user_id, friend_id) VALUES ('" + \
                                cleanStr4SQL(data['user_id']) + "','" + friend + "');"

                try:
                    cur.execute(friends_sql_str)
                except:
                    print("Insert to friends failed!")
                    print(friends_sql_str)
                conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    f.close()


def insertReviews():
    print("inserting reviews")
    # reading the JSON file
    with open(os.path.dirname(os.path.realpath(__file__)) + '\\yelpData\\yelp_review.JSON',
              'r') as f:
        line = f.readline()
        count_line = 0

        try:
            conn = psycopg2.connect("dbname='yelp' user='postgres' host='localhost' password='admin'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            review_sql_str = "INSERT INTO Reviews(review_id, user_id, business_id, stars, date, " \
                            "cool, funny, useful, text) " \
                            "VALUES ('" + cleanStr4SQL(data['review_id']) + "','" + \
                            cleanStr4SQL(data["user_id"]) + "','" + cleanStr4SQL(data["business_id"]) \
                            + "'," + str(data["stars"]) + ",'" + cleanStr4SQL(data["date"]) + "'," + \
                            str(data["cool"]) + "," + str(data["funny"]) + "," + \
                            str(data["useful"]) + ",'" + cleanStr4SQL(data["text"]) + "');"
            try:
                cur.execute(review_sql_str)
            except:
                print("Insert to review table failed!")
                print(review_sql_str)
            conn.commit()

            # optionally you might write the INSERT statement to a file.
            # outfile.write(businesses_sql_str)

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    f.close()


def insertCheckins():
    print("Inserting checkins")
    # read the JSON file
    with open(os.path.dirname(os.path.realpath(__file__)) + '\\yelpData\\yelp_checkin.JSON', 'r') as f:

        line = f.readline()
        count_line = 0

        try:
            conn = psycopg2.connect("dbname='yelp' user='postgres' host='localhost' password='admin'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        # read each JSON object and extract data
        while line:
            data = json.loads(line)
            business_id = str(cleanStr4SQL(data['business_id']))
            for day in data['time']:
                morning = 0
                afternoon = 0
                evening = 0
                night = 0
                for hour in data['time'][day]:
                    if hour in ['6:00', '7:00', '8:00', '9:00', '10:00', '11:00']:
                        morning += data['time'][day][hour]
                    elif hour in ['12:00', '13:00', '14:00', '15:00', '16:00']:
                        afternoon += data['time'][day][hour]
                    elif hour in ['17:00', '18:00', '19:00', '20:00', '21:00', '22:00']:
                        evening += data['time'][day][hour]
                    elif hour in ['23:00', '0:00', '1:00', '2:00', '3:00', '4:00', '5:00']:
                        night += data['time'][day][hour]

                checkin_sql_str = "INSERT INTO CheckIns(business_id, day, morning, " \
                                  "afternoon, evening, night) VALUES ('" + \
                                  str(business_id) + "','" + str(day) + "'," + str(morning) + "," + \
                                  str(afternoon) + "," + str(evening) + "," + str(night) + ");"
                try:
                    cur.execute(checkin_sql_str)
                except:
                    print("Insert to checkin table failed!")
                    print(checkin_sql_str)
                conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()
    print(count_line)
    f.close()


insertBussinessHoursCategories()
insertUsers()
insertFriends()
insertReviews()
insertCheckins()
